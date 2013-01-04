var student_grouping = student_grouping || {};

student_grouping.multipleGroupEditMain = function () {
    var me = this;
    this.pubSub = PubSub;  

    this.mainContentClass = '.main-content';
    this.userInactivityModal = "#user-inactivity-modal";
    this.userInactivityOkBtn = ".user-inactivity-ok";

    this.init = function () {
        // load main widgets onto app namespace
        student_grouping.mgeTopBarWidgetComponent = new student_grouping.multipleGroupEditTopbarWidget();
        student_grouping.studentFilterWidgetComponent = new student_grouping.studentFilterWidget();
        student_grouping.studentListWidgetComponent = new student_grouping.studentListWidget();
        student_grouping.groupListWidgetComponent = new student_grouping.groupListWidget();

        var loaded = false;

        // hide the top bar until we have loaded all the necessary data
        student_grouping.mgeTopBarWidgetComponent.toggleVisible(false);

        // give the loading a 60 second timeout
        setTimeout(function () {
            if (!loaded) {
                // redirect to main page for authentication if req is taking too long
                window.location = '/';
            }
        }, 60000);

        me.setupIdleTimer();

        $.ajax({
            type: 'GET',
            url: 'Group',
            success: function (data) {

                if (data.sections === undefined) {
                    window.location = "/Home";
                }

                // store lookups
                student_grouping.sections = data.sections;

                // set up the students list --> this goes before the groupsList 
                // because groupsList depends on the full list of students
                var students = data.students;
                var studentModels = _.map(students, function (student) {
                    return new student_grouping.studentModel(student);
                });

                student_grouping.studentListWidgetComponent.init(studentModels);

                // set up the groups list
                var groups = data.cohorts;
                var groupModels = _.map(groups, function (group) {
                    var groupModel = new student_grouping.groupModel(group);
                    groupModel.init();
                    return groupModel;
                });
                student_grouping.groupListWidgetComponent.init(groupModels, data.colors, data.dataElements);

                // set up the top bar controls
                student_grouping.mgeTopBarWidgetComponent.init(groupModels);

                // set up the filter components
                student_grouping.studentFilterWidgetComponent.init(data.filters);

                // set up draggables and droppables
                $(".multidraggable").multidraggable(
                {
                    drag: function (event, ui) {
                        var originalWidth = $(ui.helper.context).width();
                        $(ui.helper[0]).width(originalWidth);
                        student_grouping.groupListWidgetComponent.currGrp = null;
                    },
                    revert: "invalid",
                    "helper": "clone",
                    "opacity": 0.7,
                    appendTo: 'body'
                });

                $(me.mainContentClass).spin(false);
                $(me.mainContentClass).css('opacity', 1);

                student_grouping.mgeTopBarWidgetComponent.toggleVisible(true);

                loaded = true;
            },
            error: function (errorMsg) {
                window.location = 'Error';
                $(me.mainContentClass).spin(false);
                $(me.mainContentClass).css('opacity', 1);
            }
        });

        // set the size of page
        var windowHeight = $(window).height();
        $(me.mainContentClass).height((windowHeight - 3) + 'px');
        var studentCtrlHeight = $("#student-controls").height();
        var studentCtrlMargin = $("#student-controls").css('margin-bottom');
        var studentListMargin = $("#studentListDiv").css('margin-top');
        $("#studentListDiv").height();

        me.pubSub.subscribe('logout', me.logout);
    },

    /**
     * Detect inactivity. If user has been idle for 20 minutes,
     * warn the user and then perform a logout operation
     */
     this.setupIdleTimer = function () {
         $.idleTimer(1200000);
         $(document).bind('idle.idleTimer', function () {
             setTimeout(function () {
                 me.logout();
             }, 10000);
             $(me.userInactivityModal).modal('show');
         });
     },

    this.logout = function () {
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
}

$(function () {
    var main = new student_grouping.multipleGroupEditMain();
    main.init();
    $(".main-content").spin();
});