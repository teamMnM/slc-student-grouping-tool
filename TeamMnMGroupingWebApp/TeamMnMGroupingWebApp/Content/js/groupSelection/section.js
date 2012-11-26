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
									'<img src="" class="group-horizontal-divider"/>' +
									'<div class="group-list"> </div>' +
								'</div>';   
								
	/**************************
     * METHODS
     **************************/
    this.init = function(){
        me.sectionContainerId = "#" + me.sectionData.id;

        me.pubSub.subscribe('group-deleted', me.removeGroup);
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
}