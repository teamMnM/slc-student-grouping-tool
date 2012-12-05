var student_grouping = student_grouping || {};

student_grouping.groupsList = function(){
	var me = this;
	
	this.pubSub = PubSub;
	
	this.allGroups = [];

	// keep track of the current group we are dragging to
	this.currGrp = null;
	this.groups = [];
	this.dataElements = [];
	this.lastNewGroupIndex = -1;
	
	this.groupClass = '.group';
	this.groupsAreaClass = '.groups-area';

	this.saveAllGroupsModalElem = "#save-all-summary";
	this.saveAllGroupsContentElem = "#save-all-summary-content";    

	this.colorList = [];
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
    	
	    _.each(groups, function (group) {
	        me.allGroups.push(group);
	    });

	    // add the groups passed from the prev screen
	    var urlParams = utils.uiUtils.getUrlParams();
	    var numNewGroups = urlParams.create;
	    if (numNewGroups !== undefined && numNewGroups !== null) {
	        for (var i = 0; i < numNewGroups; i++) {
	            var newGroup = me.createNewGroupObj();                
	            var newGroupObj = me.addGroup(newGroup);
	        }
	    }

	    var selGroups = urlParams.selGroups;
	    if (selGroups !== undefined && selGroups !== null) {
	        var selGroupsArr = selGroups.split(',');
	        _.each(selGroupsArr, function (selGroupId) {
	            var selGroup = _.find(me.allGroups, function (group) {
	                return group.cohort.id === selGroupId;
	            });
	            me.addGroup(selGroup);
	        });
	    }

		this.pubSub.subscribe('add-group', function(group){
		    var newGroup = me.addGroup(group);
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
					
			    var group = me.createNewGroupObj();			    	
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
	    var groups = me.groups;

	    var groupsToSave = [];
	    var originalGroupsToSave = []; // keep track of the actual group objects so we can later update with created ID
	    _.each(groups, function (group) {
            // find dirty and new groups
	        if (group.dirty || group.groupData.id < 0) {
	            var cohortActionObject = group.prepareGroupForSaving();
	            groupsToSave.push(cohortActionObject);
	            originalGroupsToSave.push(group);
	        }
	    });

        // make sure there are dirty groups to save
	    if (groupsToSave.length === 0) {
	        alert("There are no groups to save. Please add some existing groups to modify or create new ones before saving.");
	        me.saveAllComplete();
	        return;
	    }

        // reset synching params
	    this.groupsAdded = false;
	    this.createGroupsResults = [];

	    this.groupsUpdated = false;
	    this.updateGroupsResults = [];

	    // disable the groups area screen
	    $(this.groupsAreaClass).spin();
	    $(this.groupsAreaClass).css('opacity', 0.2);

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

    /**
     * TODO refactor assign ids to new groups
     */
	this.saveAllGroupsSuccessHandler = function(results, groupsToSave){
	    var numSuccessfulSaves = 0;
	    var numResults = results.length;
	    for (var i = 0; i < numResults; i++) {
	        var result = results[i];

            // assign id to newly created groups
	        if (result.objectId !== null && result.objectId !== undefined) {
	            var groupToSave = groupsToSave[i];
	            groupToSave.updateId(result.objectId);
	        }

	        if (result.completedSuccessfully) {
	            numSuccessfulSaves++;
	        }
	    }

	    $(me.saveAllGroupsContentElem).html("<div class='well label-success save-all-msg'>Number of successful saves: " + numSuccessfulSaves + 
            "</div> <div class='well label-important save-all-msg'>Number of unsuccessful saves: " + (numResults - numSuccessfulSaves) + "</div>"
        );
	    $(me.saveAllGroupsModalElem).modal('show');

	    me.saveAllComplete();
	}

    /**
     * TODO need better way to display errors
     */
	this.saveAllGroupsErrorHandler = function (errorMsg) {
	    $(me.saveAllGroupsModalElem).modal('show');
	    $(me.saveAllGroupsContentElem).html("<div>There was an error saving your changes.</div> <div>Error message: " + errorMsg.responseText + "</div>");

	    me.saveAllComplete();
	}

	this.saveAllComplete = function () {

	    // notify others that save all has completed
	    me.pubSub.publish('save-all-completed');

	    // re-enable the groups area screen
	    $(this.groupsAreaClass).spin(false);
	    $(this.groupsAreaClass).css('opacity', 1);
	}

    /**
     * Creates a new empty group object
     */
	this.createNewGroupObj = function () {
	    var group = {
	        cohort: {
	            id: me.lastNewGroupIndex--,
	            cohortIdentifier: 'New Group',
	            cohortDescription: ''
	        },
	        students: [],
	        custom: {
	            lessonPlan: null,
	            dataElements: null
	        }
	    };
	    return group;
	}
}
