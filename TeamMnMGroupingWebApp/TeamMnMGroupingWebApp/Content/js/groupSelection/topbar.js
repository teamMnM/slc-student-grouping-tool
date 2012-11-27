var group_selection = group_selection || {};

group_selection.topbar = function(){
	var me = this;
	this.pubSub = PubSub;
	
	this.groups = [];
	
	this.groupSearchTxtElem = "#group-search-txt";
	
	 /**************************
     * METHODS
     **************************/                      
     this.init = function(groups){
     	me.groups = groups;
     	
     	_.each(groups, function(group){
     		$(me.groupSearchTxtElem).append('<option value="' + group.cohort.id +'">' + group.cohort.cohortIdentifier + '</option>');
     	});
     	
     	$(me.groupSearchTxtElem).select2( {width:'element'} );     	     	
     }
}
