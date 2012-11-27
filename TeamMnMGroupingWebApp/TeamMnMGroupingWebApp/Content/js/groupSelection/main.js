var group_selection = group_selection || {};

//group_selection.pubSub = PubSub;

group_selection.groupSectionListComponent = new group_selection.groupSectionList();
group_selection.groupDetailsComponent = new group_selection.groupDetails();
group_selection.topbarComponent = new group_selection.topbar();
group_selection.sections = [];

group_selection.init = function() {
	
	var me = this; 
	
	// set the size of page
	var windowHeight = $(window).height();
	$(".main-content").height((windowHeight-3) + 'px');
	
	// grab the data from the server
	$.ajax({
		type: 'GET',
		url: 'Group',
		success: function(data) {
			
			me.sections = data.sections;
			me.groupSectionListComponent.init(data.cohorts);
			me.groupDetailsComponent.init(data.students, data.dataElements);
			me.topbarComponent.init(data.cohorts);
		}
	})
	
}

// initialize module
$(function() {
	group_selection.init();
});
