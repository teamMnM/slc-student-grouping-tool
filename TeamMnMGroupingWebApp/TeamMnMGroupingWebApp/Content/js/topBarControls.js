var student_grouping = student_grouping || {};

student_grouping.topBarControls = function(){
	
    var me = this;
    
    this.pubSub = PubSub;
    
	this.backBtnElem = '#back-btn';
	this.findGroupDropdownElem = '#find-group-dropdown';
	this.findGroupSelect2Elem = '#s2id_find-group-dropdown'; // this could change with plugin update - unstable
	this.addExistingGroupBtn = '#add-existing-group-btn';
	this.addNewGroupBtn = '#add-new-group-btn';
	this.printBtnElem = '#img-print-btn';
	this.saveAllBtnElem = '#img-save-btn';
	this.logoutBtnElem = '#logout-btn';
	
	this.savingAll = false;
	this.groups = [];
	
	/**************************
     * METHODS
     **************************/
    this.init = function(groups){
    	
    	// set up the existing groups dropdown
    	_.each(groups, function(group){
    	    me.groups.push(group);
    	    var cohortData = group.cohort;
    		$(me.findGroupDropdownElem).
    			append("<option value='" + cohortData.id + "'>" + cohortData.cohortIdentifier + "</option>");
    	});    	    
    	
    	$(this.findGroupDropdownElem).select2();
    	setTimeout(function () {
    	    $(".select2-container").not('.span11').width('100%');
    	}, 500);
    	
    	$(this.backBtnElem).click(this.navigateBack);
    	$(this.addExistingGroupBtn).click(this.addExistingGroup);
    	$(this.addNewGroupBtn).click(this.addNewGroup);
    	$(this.printBtnElem).click(this.printAllGroups);
    	$(this.saveAllBtnElem).click(this.saveAllGroups);
    	$(this.logoutBtnElem).click(this.logout);

        // remove group from dropdown if deleted
    	this.pubSub.subscribe('group-deleted', function (id) {
    	    me.removeGroup(id);
    	});

        // TODO add description
    	this.pubSub.subscribe('save-all-completed', function () {
    	    me.savingAll = false;
    	});

        // add newly created (saved to server) group to dropdown list
    	this.pubSub.subscribe('add-to-existing-groups', function (group) {
    	    $(me.findGroupDropdownElem).
    			append("<option value='" + group.cohort.id + "'>" + group.cohort.cohortIdentifier + "</option>");
    	    me.groups.push(group);
    	});

    }
    
    /**
     *  
     */
    this.addExistingGroup = function(event){
    	
    	// check if a group has been selected
    	var selGroupId = $(me.findGroupDropdownElem).val();
    	var groupAdded = student_grouping.groupsListComponent.containsGroup(selGroupId);
    	
    	if (selGroupId === ''){
    		// create tooltip on the fly
    		$(me.findGroupSelect2Elem).tooltip('destroy');
    		$(me.findGroupSelect2Elem).tooltip({
	    		title : 'please select a group to add',
	    		placement :'bottom',
	    		trigger :'manual'
	    	}); 
    		
    		$(me.findGroupSelect2Elem).tooltip('show');
			setTimeout(function(){
				$(me.findGroupSelect2Elem).tooltip('hide');
			}, 3000);
	    } else if (groupAdded){
	    	// create tooltip on the fly
    		$(me.findGroupSelect2Elem).tooltip('destroy');
    		$(me.findGroupSelect2Elem).tooltip({
	    		title : 'group has already been added',
	    		placement :'bottom',
	    		trigger :'manual'
	    	}); 
    		
    		$(me.findGroupSelect2Elem).tooltip('show');
			setTimeout(function(){
				$(me.findGroupSelect2Elem).tooltip('hide');
			}, 3000);
	    } else {
	    	   
	    	// get the group obj
	    	var group = _.find(me.groups, function(g){
	    		return g.cohort.id === selGroupId;
	    	});
	    	
	    	me.pubSub.publish('add-group', group);
	    	
	    	// clear the dropdown
	    	$(me.findGroupDropdownElem).select2('val', '');
	    }
    }
    
    /**
     * Adds a new group to the list (unsaved)
     */
    this.addNewGroup = function(event){
    	var group = {
    	    cohort: {
    	        id: student_grouping.groupsListComponent.lastNewGroupIndex--,
    	        cohortIdentifier: 'New Group',
    	        cohortDescription: ''
    	    },
    	    students: [],
    	    custom: {
    	        lessonPlan: null
    	    }
    	};
    	
    	me.pubSub.publish('add-group', group);
    }

    /**
     * Remove the given group from the list and dropdown
     */
    this.removeGroup = function (groupId) {
        this.groups = _.filter(this.groups, function(g){
            return g.cohort.id !== groupId;
        });

        // remove from dropdown list
        $(me.findGroupDropdownElem).find('option[value="' + groupId + '"]').remove();
    }

    /**
     * Handle click of the save all button
     */
    this.saveAllGroups = function () {

        if (!me.savingAll) {
            me.savingAll = true; // prevent user from trigger save all while saving
            me.pubSub.publish('save-all-groups');
        }
    }

    /**
     * Handle click of the print all groups button
     */
    this.printAllGroups = function () {
        if (!me.savingAll) {
            me.pubSub.publish('print-all-groups');
        }
    }

    /**
     * Navigates back to the first screen
     */
    this.navigateBack = function () {
        me.pubSub.publish('has-dirty-groups-huh', function (isDirty) {
            if (isDirty) {
                var confirmation = confirm("There are unsaved changes. Would you still like to navigate back?");
                if (!confirmation) {
                    return;
                }
            }
            window.location = "GroupSelection";
        });
    }

    /**
     *
     */
    this.logout = function () {
        var hasDirtyGroups = student_grouping.groupsListComponent.hasDirtyGroups();
        if (hasDirtyGroups) {
            var confirmDirtyLogout = confirm("You have unsaved changes. Would you still like to log out?");
            if (!confirmDirtyLogout) {
                return;
            }
        }
        me.pubSub.publish('logout');
    }
}
