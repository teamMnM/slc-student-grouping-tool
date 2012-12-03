var group_selection = group_selection || {};

group_selection.topbar = function(){
	var me = this;
	this.pubSub = PubSub;
	
	this.groups = [];
	
	this.editMultipleGroupsBtn = "#edit-multiple-groups-btn";
	this.groupSearchTxtElem = "#group-search-txt";
	this.groupSearchBtnClass = ".group-search-btn";
	this.groupSearchClearBtnClass = ".group-search-clear-btn";
	this.createGroupsBtn = '#create-groups-btn';
	this.numGroupsCreateTxt = '.num-groups-create-txt';
	this.logoutBtnClass = '.logout-btn';
	
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

     	$(me.editMultipleGroupsBtn).click(function (event) {
     	    me.editMultipleGroups();
     	});

     	$(me.logoutBtnClass).click(function (event) {
     	    me.pubSub.publish('logout');
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
         window.location = "MultipleGroupsEdit?create=" + numGroups;
     }

    /**
     * 
     */
     this.editMultipleGroups = function () {
         me.pubSub.publish('edit-multiple-groups');
     }
}
