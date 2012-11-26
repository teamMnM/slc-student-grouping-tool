var group_selection = group_selection || {};

group_selection.groupSectionList = function(){
	var me = this;
	this.pubSub = PubSub;
	
	this.groupSectionList = '.group-section-list';
	this.sections = [];
	
	this.newSectionId = 1;
	/**************************
     * METHODS
     **************************/
    this.init = function(groups){
    	var sections = [];
    	
    	_.each(groups, function(group){   
    		var g = new group_selection.group(group); 		
    		me.addGroup(g);
    	});    	
    }
    
    /**
     * Add the group to the appropriate section 
     */
    this.addGroup = function(group){
    	var custom = group.group.custom; 
    	var key = custom.lastModified;
		var section = me.sections[key];
		if (section === undefined || section === null){
			var sectionInfo = {
				id : me.newSectionId++,
				title: "Last modified " + custom.lastModified
			}
			section = new group_selection.groupSection(sectionInfo);			
			section.init();
			me.sections[key] = section;
			
			$(me.groupSectionList).append(section.generateTemplate());
		}
		section.addGroup(group);
    }
}
