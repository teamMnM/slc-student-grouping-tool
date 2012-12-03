var group_selection = group_selection || {};

group_selection.pubSub = PubSub;

group_selection.groupSectionListComponent = new group_selection.groupSectionList();
group_selection.groupDetailsComponent = new group_selection.groupDetails();
group_selection.topbarComponent = new group_selection.topbar();
group_selection.sections = [];

group_selection.mainContent = "#group-selection";

group_selection.init = function() {
	
	var me = this; 
	
	// set the size of page
	var windowHeight = $(window).height();
	$(".main-content").height((windowHeight-3) + 'px');
	
	group_selection.setupIdleTimer();

	// grab the data from the server
	$.ajax({
	    type: 'GET',
	    url: 'Group',
	    success: function (data) {

	        if (data.sections === undefined) {
	            window.location = "/Home";
	        }

	        me.sections = data.sections;
	        me.groupSectionListComponent.init(data.cohorts);
	        me.groupDetailsComponent.init(data.students, data.dataElements);
	        me.topbarComponent.init(data.cohorts);

	        $(group_selection.mainContent).spin(false);
	        $(group_selection.mainContent).css('opacity', 1);
	    },
	    error: function (errorMsg) {
	        $(group_selection.mainContent).spin(false);
	        $(group_selection.mainContent).css('opacity', 1);
	        window.location = 'Error';
	    }
	});
	

	me.pubSub.subscribe('logout', group_selection.logout);
}

group_selection.setupIdleTimer = function () {
    $.idleTimer(1200000);
    var confirmActivity = false;
    $(document).bind('idle.idleTimer', function () {
        setTimeout(function () {
            if (!confirmActivity) {
                group_selection.logout();
            }
        }, 5000);
        var confirmActivity = confirm("You have been idle for 20 minutes. Please click continue OK to extend your session.");
        if (!confirmActivity) {
            group_selection.logout();
        }
    });
}


/**
     * Log the user out, kill the session
     */
group_selection.logout = function () {
    $.ajax({
        type: 'GET',
        url: 'Logout',
        success: function (result) {
            if (result.logout) {
                window.location = "/Home";
            } else {
                window.location = "Error";
            }
        },
        error: function (result) {
            window.location = "Error";
        }
    });
}

// initialize module
$(function() {
    group_selection.init();
    $(group_selection.mainContent).spin();
});
