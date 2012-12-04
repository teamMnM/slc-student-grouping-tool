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
    this.addGroup = function(group){
    	me.groups.push(group);
    	
    	var groupTemplate = group.generateTemplate();
    	$(me.sectionContainerId).find(me.groupListClass).append(groupTemplate);
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
    this.moveGroupToTop = function (groupId) {
        var groupLi = $(me.sectionContainerId).find(me.groupListClass).find("#" + groupId);
        if (groupLi.length !== 0) {
            $(me.sectionContainerId).find(me.groupListClass).prepend(groupLi);
        }
    }    
}