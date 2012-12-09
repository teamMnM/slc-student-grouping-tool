var group_selection = group_selection || {};

group_selection.groupSectionList = function(){
	var me = this;
	this.pubSub = PubSub;
	
	this.groupSectionList = '.group-section-list';
	this.editMultipleGroupsBtn = "#edit-multiple-groups-btn";
	this.sections = [];
	
	this.newSectionId = 1;
	/**************************
     * METHODS
     **************************/
    this.init = function(groups){
    	var sections = [];
    	
    	groups = _.sortBy(groups, function(group){
    	    return parseInt(group
                .custom.lastModifiedDate
                .replace('/Date(', '')
                .replace(')/', ''));
    	});
    	groups.reverse();

    	_.each(groups, function(group){   
    		var g = new group_selection.group(group); 		
    		me.addGroup(g);
    	});

    	me.pubSub.subscribe('edit-multiple-groups', me.editMultipleGroups);
    	me.pubSub.subscribe('add-new-group', me.addGroup);

    	$(".group-section-div .box-wrap").antiscroll();
    }
    
    /**
     * Add the group to the appropriate section 
     */
    this.addGroup = function(group){
        var custom = group.group.custom;
        var lastModifiedDate = new Date(parseInt(custom.lastModifiedDate.replace('/Date(', '').replace(')/', '')));
        var key = lastModifiedDate.toFormat('DDDD, MMM DD, YYYY');
		var section = me.sections[key];
		if (section === undefined || section === null){
			var sectionInfo = {
				id : me.newSectionId++,
				title: "Last modified " + key,
                date: lastModifiedDate
			}
			section = new group_selection.groupSection(sectionInfo);			
			section.init();
			me.sections[key] = section;
			
			$(me.groupSectionList).append(section.generateTemplate());
		}
		section.addGroup(group);
    }

    /**
     *
     */
    this.editMultipleGroups = function () {
        var selGroups = [];
        for (var i in me.sections) {
            var section = me.sections[i];
            selGroups.push.apply(selGroups,section.getSelectedGroups());
        }

        // make sure groups have been selected
        if (selGroups.length === 0) {
            utils.uiUtils.showTooltip(
                        $(me.editMultipleGroupsBtn),
                        'Please select some groups for editing.',
                        'right',
                        'manual',
                        3000);
            return;
        }

        var selGroupIds = _.map(selGroups, function (selGroup) {
            return selGroup.groupData.id;
        });

        var selGroupIdsStr = selGroupIds.join(",");
        window.location = 'MultipleGroupsEdit?selGroups=' + selGroupIdsStr;
    }
}
