var student_grouping = student_grouping || {};

// TODO create a js file for all the different events out there
student_grouping.pubSub = PubSub;

student_grouping.topBarComponent = new student_grouping.topBarControls();
student_grouping.filterComponent = new student_grouping.filters();
student_grouping.studentsListComponent = new student_grouping.studentsList();
student_grouping.groupsListComponent = new student_grouping.groupsList();

// TODO create lookup component 
student_grouping.sections = [];

/**
 * TODO add description and externalize element selectors
 */
student_grouping.init = function(){
    var loaded = false;
    var me = this;
    $(".top-bar-controls").hide();

    // give it a 60 second timeout
    setTimeout(function () {
        if (!loaded) {
            // redirect to main page for authentication if req is taking too long
            window.location = '/';
        }
    }, 60000);

	$.ajax({
        type: 'GET',
        url: 'Group',
        success: function (data) {

            if (data.sections === undefined) {
                window.location = "/Home";
            }

            // store lookups
            me.sections = data.sections;

            // set up the students list --> this goes before the groupsList 
            // because groupsList depends on the full list of students
            me.studentsListComponent.init(data.students);

            // set up the groups list
            me.groupsListComponent.init(data.cohorts, data.colors, data.dataElements);

            // set up the top bar controls
            me.topBarComponent.init(data.cohorts);

            // set up the filter components
            student_grouping.filterComponent.init(data.filters);

            // set up the list controls	
            var listStudentData = _.pluck(me.students, 'studentData');

            // set up draggables and droppables
            $(".multidraggable").multidraggable(
            {
                drag: function (event, ui) {
                    var originalWidth = $(ui.helper.context).width();
                    $(ui.helper[0]).width(originalWidth);
                    student_grouping.groupsListComponent.currGrp = null;
                },                
                revert: "invalid",
                "helper": "clone",
                "opacity": 0.7,
                appendTo: 'body'
            });

            $(".main-content").spin(false);
            $(".main-content").css('opacity', 1);

            $(".top-bar-controls").show();

            loaded = true;
        },
        error: function (errorMsg) {
            window.location = 'Error';
            $(".main-content").spin(false);
            $(".main-content").css('opacity', 1);
        }
	});

    // set the size of page
	var windowHeight = $(window).height();
	$(".main-content").height((windowHeight-3) + 'px');
	var studentCtrlHeight = $("#student-controls").height();
	var studentCtrlMargin = $("#student-controls").css('margin-bottom');
	var studentListMargin = $("#studentListDiv").css('margin-top');
	$("#studentListDiv").height();

	me.pubSub.subscribe('logout', me.logout);
}

student_grouping.logout = function () {
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
    student_grouping.init();
    $(".main-content").spin();
});
