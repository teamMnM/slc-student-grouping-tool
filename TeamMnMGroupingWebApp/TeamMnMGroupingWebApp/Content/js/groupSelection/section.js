var group_selection = group_selection || {};

group_selection.groupSection = function(section){
	var me = this;
	this.pubSub = PubSub;
	
	this.sectionData = section;
	this.groups = [];
	
	this.groupSectionClass = '.group-section';
		
	this.sectionContainerId = '';
	
	this.groupListClass = '.group-list';
	this.groupSectionTitleClass = '.group-section-title';
	this.groupSectionTemplate = '<div class="group-section">' + 
									'<div class="group-section-title"></div>' +
									'<div class="group-list"> </div>' +
								'</div>';
	this.groupListClass = ".group-list";
    this.groupListItemClass = ".group-list-item";
								
	/**************************
     * METHODS
     **************************/
    this.init = function(){
        me.sectionContainerId = "#" + me.sectionData.id;

        me.pubSub.subscribe('group-deleted', me.removeGroup);

        me.pubSub.subscribe('filter-group', me.filterGroup);

        me.pubSub.subscribe('reorder-group', me.moveGroupToTop);
    }	 
    
    /**
     * Add the given group to this seciton 
     */
    this.addGroup = function (group) {
        var firstGroup = me.groups[0];
        me.groups.push(group);

    	var groupTemplate = group.generateTemplate();

        // check if the new group was modified at a later date than the first group in the list,
        // then insert at the beginning of the list
    	if (firstGroup !== undefined) {
    	    var firstGroupDate = new Date(parseInt(firstGroup.group.custom.lastModifiedDate.replace('/Date(', '').replace(')/', '')));
    	    var groupDate = new Date(parseInt(group.group.custom.lastModifiedDate.replace('/Date(', '').replace(')/', '')));

    	    if (groupDate.isAfter(firstGroupDate)) {
    	        $(me.sectionContainerId).find(me.groupListClass).prepend(groupTemplate);
    	    } else {
    	        $(me.sectionContainerId).find(me.groupListClass).append(groupTemplate);
    	    }
    	} else {
    	    $(me.sectionContainerId).find(me.groupListClass).append(groupTemplate);
    	}
    	group.init();    	
    }
    
    /**
     * Remove the given group from the list
     */
    this.removeGroup = function(groupId){
    	me.groups = _.filter(me.groups, function(g){
    		return g.groupData.id !== groupId;
    	});
    }
    
    /**
     * Return the HTML content for this object 
     */
    this.generateTemplate = function(){
    	var sectionData = me.sectionData;	
    	var template = $(me.groupSectionTemplate);
    	$(template).attr('id', sectionData.id);
    	
    	$(template).find(me.groupSectionTitleClass).html(sectionData.title);
    	return template;
    }

    /**
     * Filter the list of groups by name
     */
    this.filterGroup = function (groupName) {
        var groups = me.groups;
        var containsGroup = false;
        _.each(groups, function (group) {
            if (group.groupData.cohortIdentifier
                .toLowerCase()
                .indexOf(groupName.toLowerCase()) !== -1) {
                containsGroup = true;
                group.toggleVisible(true);
            } else {
                group.toggleVisible(false);
            }
        });

        if (!containsGroup) {
            me.toggleVisible(false);
        } else {
            me.toggleVisible(true);
        }
    }

    /**
     * Hide/show section
     */
    this.toggleVisible = function (visible) {
        if (visible) {
            $(me.sectionContainerId).show();
        } else {
            $(me.sectionContainerId).hide();
        }
    }

    /**
     * Return the ids of the selected groups
     */
    this.getSelectedGroups = function () {
        var selGroups = _.filter(me.groups, function (group) {
            return group.isSelected();
        });
        return selGroups;
    }

    /**
     *
     */
    this.moveGroupToTop = function (group) {

        var matchingGroup = _.find(me.groups, function (g) {
            return g.groupData.id === group.cohort.id;
        });

        if (matchingGroup !== undefined && matchingGroup !== null) {            
            // determine if group should be in this section or move to a new one
            var sectionDate = me.sectionData.date;
            var groupDate = new Date(parseInt(group.custom.lastModifiedDate.replace('/Date(', '').replace(')/', '')));

            if (sectionDate.getDate() !== groupDate.getDate()) {
                me.groups = _.reject(me.groups, function (g) {
                    return g.groupData.id === group.cohort.id;
                });
                matchingGroup.remove();
                me.pubSub.publish('add-new-group', matchingGroup);
            } else {
                // get the list item html
                var groupLi = $(me.sectionContainerId).find(me.groupListClass).find("#" + group.cohort.id);

                // move to the top of the list in this section as this would be the most recently update group
                $(me.sectionContainerId).find(me.groupListClass).prepend(groupLi);
            }
        }
    }    
}