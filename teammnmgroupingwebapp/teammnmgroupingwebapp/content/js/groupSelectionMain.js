var student_grouping = student_grouping || {};

student_grouping.groupSelectionMain = function () {
    var me = this;
    this.pubSub = PubSub;

    this.mainContent = "#group-selection";
    this.userInactivityModal = "#user-inactivity-modal";
    this.userInactivityOkBtn = ".user-inactivity-ok";

    /**
     * Initialize the app
     */
    this.init = function () {
        student_grouping.sectionListWidgetComponent = new student_grouping.sectionListWidget();
        student_grouping.groupDetailsWidgetComponent = new student_grouping.groupDetailsWidget();
        student_grouping.groupSelectionTopbarWidgetComponent = new student_grouping.groupSelectionTopbarWidget();

        // set the size of page
        var windowHeight = $(window).height();
        $(".main-content").height((windowHeight - 3) + 'px');

        me.setupIdleTimer();

        // grab the data from the server
        $.ajax({
            type: 'GET',
            url: 'Group',
            success: function (data) {

                if (data.sections === undefined) {
                    window.location = "/Home";
                }

                me.sections = data.sections;

                // setup the groups list
                var groups = data.cohorts;
                var groupModels = _.map(groups, function (group) {
                    var groupModel = new student_grouping.groupModel(group);
                    groupModel.init();
                    return groupModel;
                });
                student_grouping.sectionListWidgetComponent.init(groupModels);

                // setup the group details component
                var students = data.students;
                var studentModels = _.map(students, function (student) {
                    return new student_grouping.studentModel(student);
                });
                student_grouping.groupDetailsWidgetComponent.init(studentModels, data.dataElements);

                student_grouping.groupSelectionTopbarWidgetComponent.init();
                $(me.mainContent).spin(false);
                $(me.mainContent).css('opacity', 1);
            },
            error: function (errorMsg) {
                $(me.mainContent).spin(false);
                $(me.mainContent).css('opacity', 1);
                window.location = 'Error';
            }
        });

        $(me.userInactivityOkBtn).click(function (event) {
            me.logout();
        });

        me.pubSub.subscribe('logout', me.logout);
    }

    this.setupIdleTimer = function () {
        $.idleTimer(1200000);
        $(document).bind('idle.idleTimer', function () {
            setTimeout(function () {
                me.logout();
            }, 10000);
            $(me.userInactivityModal).modal('show');
        });
    }

    /**
         * Log the user out, kill the session
         */
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

// initialize module
$(function () {
    var main = new student_grouping.groupSelectionMain();
    main.init();
    $(main.mainContent).spin();
});