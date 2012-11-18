var student_grouping = student_grouping || {};


student_grouping.pubSub = PubSub;

student_grouping.topBarComponent = new student_grouping.topBarControls();
student_grouping.filterComponent = new student_grouping.filters();
student_grouping.studentsListComponent = new student_grouping.studentsList();
student_grouping.groupsListComponent = new student_grouping.groupsList();

student_grouping.init = function(){
		
    var me = this;

	$.ajax({
        type: 'GET',
        url: 'Group',
        success: function (data) {


            // set up the students list --> this goes before the groupsList 
            // because groupsList depends on the full list of students
            me.studentsListComponent.init(data.students);

            // set up the groups list
            me.groupsListComponent.init(data.cohorts, data.colors);

            // set up the top bar controls
            me.topBarComponent.init(data.cohorts);

            // set up the filter components
            student_grouping.filterComponent.init(data.filters);

            _.each(data.dataElements, function (dataElement) {
                var dataElem = $("<li><input class='cbox-student-attribute' type='checkbox'"
                    + "value='" + dataElement.attributeId + "' data-displayName='"
                    + dataElement.attributeName + "'/>" + dataElement.attributeName
                    + "</li>");
                $(".student-data-popover .student-elements-list").append(dataElem);
            });

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
            $(".main-content").css('opacity',1 );
        },
        error: function (errorMsg) {
            window.location = 'Index';
            $(".main-content").spin(false);
            $(".main-content").css('opacity', 1);
        }
	});

    // set the size of page
	var windowHeight = $(window).height();
	$(".main-content").height(windowHeight + 'px');
	var studentCtrlHeight = $("#student-controls").height();
	var studentCtrlMargin = $("#student-controls").css('margin-bottom');
	var studentListMargin = $("#studentListDiv").css('margin-top');
    $("#studentListDiv").height()
}

// initialize module
$(function() {
    student_grouping.init();
    $(".main-content").spin();
});
