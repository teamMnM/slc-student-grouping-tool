var student_grouping = student_grouping || {};

student_grouping.groupsList = function(){
	var me = this;
	
	this.pubSub = PubSub;
	
	// keep track of the current group we are dragging to
	this.currGrp = null;
	this.groups = [];
	this.dataElements = [];
	this.lastNewGroupIndex = -1;
	
	this.groupClass = '.group';
	this.groupsAreaClass = '.groups-area';

	this.saveAllGroupsModalElem = "#save-all-summary";
	this.saveAllGroupsContentElem = "#save-all-summary-content";    

    // colors for the groups
	this.colorList = [
        /*{ background: '#DBFDAA', title: '#7D9D38' },
        { background: '#A5C5FF', title: '#2F62A0' },
        { background: '#FFA5A4', title: '#A9322F' },
        { background: '#CBB7E9', title: '#654788' },
        { background: '#A7ECFF', title: '#35ADCD' },
        { background: '#FFC08A', title: '#F79646' },
        { background: '#D5D5D5', title: '#000000' }*/        
	];
	this.currentColorIndex = 0;
	
	/**************************
     * METHODS
     **************************/
	this.init = function (groups, colors, dataElements) {

	    // store pre-defined data elements from backend
	    _.each(dataElements, function (dataElement) {
	        me.dataElements.push(dataElement);
	    });

        // store pre-defined colors from backend
	    _.each(colors, function (color) {
	        me.colorList.push(color);
	    });
    	
		for (var i = 0; i < groups.length; i++){
			var group = groups[i];
			this.addGroup(group);			
		}		

		this.pubSub.subscribe('add-group', function(group){
		    var newGroup = me.addGroup(group);
		    newGroup.markDirty();
		});
		
		this.pubSub.subscribe('group-removed', function(groupId){
			me.removeGroup(groupId);
		});
		
		this.pubSub.subscribe('group-deleted', function (groupId) {
		    me.removeGroup(groupId);
		});

		this.pubSub.subscribe('assign-random', function(students, numInGroup){
			me.assignRandomGroups(students, numInGroup);
		});

		this.pubSub.subscribe('save-all-groups', function () {
		    me.saveAllGroups();
		});
    }
    
    /**
     * Add given group to the list 
     * @return the newly added group
     */
    this.addGroup = function(newGroup){
    	
    	// check if group has already been added
    	var groupExists = _.find(this.groups, function(g){
    		return g.groupData.id === newGroup.id;
    	}) !== undefined;
    	    	
    	if (!groupExists) {    	
    	    var group = new student_grouping.group(newGroup);
    	    var color = this.colorList[this.currentColorIndex++];
			$(this.groupsAreaClass).append(group.generateTemplate(color));
			group.init(me.dataElements);
			
			if (this.currentColorIndex >= this.colorList.length) {
			    this.currentColorIndex = 0;
			}

			// make group droppable
			$(group.groupContainerId).find('.group').droppable({
				drop: function(event){
					me.dropFunc(this);
				}		
			});	
			
			this.groups.push(group);
			
			return group;
		} else {
			//TODO tell user group is already there
		}
		
		return null;
    }
    
    /**
     * Remove the given group from the list 
     */
    this.removeGroup = function(groupId){
    	this.groups = _.filter(this.groups, function(g){
    		return g.groupData.id !== groupId;
    	});
    }
    
    /**
	 *Handle the drop action on a group 
	 */
	this.dropFunc = function(groupContainer){
		// we use currGrp to make sure that the students get dropped once
		// need to do this because the dropFunc is called for each dragged student
		if (me.currGrp === null){
			var groupId = $(groupContainer).attr('id');
			me.currGrp = groupId;
			
			// find the group object
			var group = _.find(me.groups, function(group){
				return group.groupData.id.toString() === groupId;
			});
			
			// loop through each student being dragged
			$(".ui-draggable-dragging").each(function(index, elem){
							
				var studentId = $(elem).attr('data-studentId');
					
				// find student obj TODO - refactor dependency
				var student = _.find(student_grouping.studentsListComponent.students, function(student){
					return student.studentData.id === studentId;
				});
				
				// check if are moving between groups
				if ($(elem).hasClass('dropped-elem')){
									
					// find the group me belongs to 
					var originalGroupId = $(elem).parents('.group').attr('id');
									
					// check if target group is indeed a different group
					if (originalGroupId !== groupId) {				
						// find the group object
						var originalGroup = _.find(me.groups, function(g){
							return g.groupData.id.toString() === originalGroupId;
						});
						
						// check that target group doesnt aleady have me student					
						var studentId = $(elem).attr('data-studentId');
						var groupHasStudent = group.hasStudent(studentId);
										
						if (!groupHasStudent) {
							originalGroup.removeStudent(studentId);		
							originalGroup.markDirty();				
						}	
					}				
				}
				
				// assign student to group
				group.assignStudentToGroup(student);
				group.markDirty();		
			});		
		}
	}
	
	/**
	 * Randomly assign the given students into groups of the specified number  
	 */
	this.assignRandomGroups = function(students, numInGroup){
		var students = utils.arrayUtils.shuffle(students);
		var groups = utils.arrayUtils.shuffle(this.groups);
		var numGroups = groups.length;
		
	    // remove students from all groups
		_.each(groups, function (group) {
		    group.removeAllStudents();
		});

		// keep track of the next group that is not FULL
		var indexOfNextAvailableGroup = 0;
		_.each(students, function(student){
			var studentId = student.studentData.id;			

			// keep track of whether student has been assigned to the existing groups
			var addedToGroup = false;			
			for (var i = indexOfNextAvailableGroup; i < numGroups; i++){
				var group = groups[i];
				if (group.students.length < numInGroup){
					group.assignStudentToGroup(student);
						
					addedToGroup = true;
					break;
				} else if (group.students.length === numInGroup){
					indexOfNextAvailableGroup++;
				}					
			}
				
			// should create a new group if student was not added to any existing group
			if (!addedToGroup){
					
				var group = {
					cohort: {
					    id: me.lastNewGroupIndex--,
					    cohortIdentifier: 'New Group',
					    cohortDescription: ''
					},
                    students: []
			    };
			    	
			    var newGroupObject = me.addGroup(group);
			    newGroupObject.assignStudentToGroup(student);
			    newGroupObject.markDirty();
			    numGroups++;
			}			
		});		
	}
	
	/**
	 * Returns true if the given group exists in this component's list of groups 
	 */
	this.containsGroup = function(groupId){
		
		var existingGroup = _.find(this.groups, function(g){
			return g.groupData.id === groupId;
		});
		return existingGroup !== undefined;
	}
	
    /**
     * TODO server side will handle all this with one call
     * Saving changes to all groups
     */
	this.saveAllGroups = function () {

        // reset synching params
	    this.groupsAdded = false;
	    this.createGroupsResults = [];

	    this.groupsUpdated = false;
	    this.updateGroupsResults = [];

	    var groupsToSave = [];
        
	    var groups = me.groups;
	    var originalGroupsToSave = []; // keep track of the actual group objects so we can later update with created ID
	    _.each(groups, function (group) {
	        if (group.dirty) {
	            var cohortActionObject = group.prepareGroupForSaving();
	            groupsToSave.push(cohortActionObject);
	            originalGroupsToSave.push(group);
	        }
	    });
        
	    // call server to add all new groups
	    $.ajax({
	        type: 'POST',
	        url: 'SaveAll',
	        contentType: 'application/json',
	        data: JSON.stringify(groupsToSave),
	        success: function (results) {
	            me.saveAllGroupsSuccessHandler(results, originalGroupsToSave);
	        },
	        error: me.saveAllGroupsErrorHandler
	    });        	    
	}

	this.saveAllGroupsSuccessHandler = function(results, groupsToSave){
	    var numResults = results.length;
	    for (var i = 0; i < numResults; i++) {
	        var result = results[i];
	        if (result.objectId !== null && result.objectId !== undefined){
	        }
	    }

	    $(me.saveAllGroupsContentElem).html();
	    $(me.saveAllGroupsModalElem).modal('show');
	}

	this.saveAllGroupsErrorHandler = function (errorMsg) {
	    $(me.saveAllGroupsModalElem).modal('show');
	}
}
