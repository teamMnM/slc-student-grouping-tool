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
            me.groupsListComponent.init(data.cohorts);

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
                    student_grouping.groupsListComponent.currGrp = null;
                },
                revert: "invalid",
                "helper": "clone",
                "opacity": 0.7
            });
        }
	});

	
}

// initialize module
$(function() {
	student_grouping.init();
});