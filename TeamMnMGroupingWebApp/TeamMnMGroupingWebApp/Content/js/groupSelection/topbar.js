var group_selection = group_selection || {};

group_selection.topbar = function(){
	var me = this;
	this.pubSub = PubSub;
	
	this.groups = [];
	
	this.groupSearchTxtElem = "#group-search-txt";
	this.groupSearchBtnClass = ".group-search-btn";
	this.groupSearchClearBtnClass = ".group-search-clear-btn";
	this.createGroupsBtn = '#create-groups-btn';
	this.numGroupsCreateTxt = '#num-groups-create-txt';
	
	 /**************************
     * METHODS
     **************************/                      
     this.init = function(groups){
     	me.groups = groups;
     	
     	_.each(groups, function(group){
     		$(me.groupSearchTxtElem).append('<option value="' + group.cohort.id +'">' + group.cohort.cohortIdentifier + '</option>');
     	});
     	
     	$(me.groupSearchTxtElem).select2({ width: 'element' });
     	$(me.groupSearchBtnClass).click(function (event) {
     	    me.filterGroupsByName();
     	});

     	$(me.groupSearchClearBtnClass).click(function (event) {
     	    me.clearGroupSearch();
     	});

         
     	$(me.createGroupsBtn).click(function(event){
     	    me.createGroups();
     	});
     }

    /**
     * 
     */
     this.filterGroupsByName = function () {
         var groupName = $(me.groupSearchTxtElem).select2('data').text;
         me.pubSub.publish('filter-group', groupName);
     }

     this.clearGroupSearch = function () {
         $(me.groupSearchTxtElem).select2('val', '');
         me.pubSub.publish('filter-group', '');
     }

    

    /**
     * Create the specified number of groups
     */
     this.createGroups = function () {
         var numGroups = $(me.numGroupsCreateTxt).val();

         var groupsToCreate = [];
         for (var i = 0; i < numGroups; i++) {
             var groupData = {

             };
             var newGroup = new group_selection.group(groupData);
             }
     }
}
