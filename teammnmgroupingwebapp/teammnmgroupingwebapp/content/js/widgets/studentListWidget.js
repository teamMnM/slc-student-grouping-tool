var student_grouping = student_grouping || {};

student_grouping.studentListWidget = function () {
    var me = this;
    this.pubSub = PubSub;

    this.studentModels = [];
    this.studentWidgets = [];

    this.studentListElem = '#studentList';
    this.selectAllBtn = '#select-all-btn';
    this.randomBtn = '#random-btn';
    this.randomNumTxt = '#random-num-txt';
    this.studentSearchBox = '#txtStudentSearchBox';

    // keep track of whether user has selected all
    this.allSelected = false;

    /**************************
     * SETUP METHODS
     **************************/
    /**
     * Initialize this widget
     * @params students - server-side students
     */
    this.init = function (studentModels) {

        // add all students to the list
        for (var i = 0; i < studentModels.length; i++) {
            var studentModel = studentModels[i];
            
            var studentListItemWidget = new student_grouping.studentListItemWidget(studentModel);
            $(me.studentListElem).append(studentListItemWidget.generateTemplate());
            studentListItemWidget.init();

            me.studentModels.push(studentModel);
            me.studentWidgets[studentModel.getId()] = studentListItemWidget;
        }

        this.setupEventHandlers();
        this.setupSubscriptions();

        $('#studentListDiv .box-wrap').antiscroll();

        // if firefox or IE, fix the width of the antiscroll-inner to hide the default scrollbars	
        if (!$.browser.webkit) {
            var listWidth = $('#studentListDiv').find('.antiscroll-inner').width();
            $('#studentListDiv').find('.antiscroll-inner').css('width', listWidth + 4);
        }

    }

    /**
     * Sets up the event handlers for user interaction with the widget
     */
    this.setupEventHandlers = function () {
        // add event handler for filter student list using search box
        $(me.studentSearchBox).keyup(function () {
            var filterVal = $(this).val();
            var filter = {
                attributeName: 'name',
                attributeId: 'name',
                operator: 'matches',
                value: filterVal,
                values: []
            }

            // notify filter component to add filter for 
            me.pubSub.publish('add-manual-filter', filter);
        });

        $(me.selectAllBtn).click(function (event) {
            me.selectAllStudents();
        });

        $(me.randomBtn).click(function (event) {
            me.assignRandom();
        });
    }

     /**
      * Sets up listeners for pubsub events
      */
    this.setupSubscriptions = function () {

        // TODO add description
        me.pubSub.subscribe('filter-student-list', function () {
            me.filterStudentList();
        });

        me.pubSub.subscribe('student-selection-changed', function (studentId) {
            // TODO add method
        });
    }

    /**************************
     * METHODS
     **************************/
    /**
	 * Repopulate the list of students with given list 
     * TODO method is not used, need to set up
	 */
    this.changeSelectableList = function (listStudentData, changeEventHandler) {
        var options = [];
        _.each(listStudentData, function (studentData) {
            return options.push(
				{ id: studentData.id, text: studentData.name });
        });

        $(me.studentSearchBox).select2('destroy');
        $(me.studentSearchBox).select2(
			{
			    data: options,
			    width: 'element'
			});
        $(me.studentSearchBox).on('change', changeEventHandler);
    }

    /**
	 * Applies the filters from the filter 
	 */
    this.filterStudentList = function () {
        // TODO refactor dependency on filter component
        var filteredStudents = student_grouping.studentFilterWidgetComponent.applyFilters(me.studentModels);
        _.each(me.studentModels, function (studentModel) {
            var studentId = studentModel.getId();
            var filteredStudent = _.find(filteredStudents, function (s) {
                return s.getId() === studentId;
            });
            var studentListItemWidget = me.studentWidgets[studentId];
            studentListItemWidget.toggleVisible(filteredStudent !== undefined);
        });

        // if the deselect all btn has been toggled, change it back to select all
        $(me.selectAllBtn).html('select all');
    }

    /**
	 * Select all students in the list 
	 */
    this.selectAllStudents = function () {
        _.each(me.studentWidgets, function (studentListItemWidget) {
            if (studentListItemWidget.visible) {
                studentListItemWidget.toggleStudentSelection(!me.allSelected);
            }
        });

        me.allSelected = !me.allSelected;

        if (me.allSelected) {
            $(me.selectAllBtn).html('deselect all');
        } else {
            $(me.selectAllBtn).html('select all');
        }
    }

    /**
	 * Randomly organizes all students into groups (reassigns students that are already
     * assigned to groups)
	 */
    this.assignRandom = function () {

        var randomNum = $(me.randomNumTxt).val().trim();
        if (isNaN(randomNum) || randomNum === '' || parseInt(randomNum) <= 0) {
            utils.uiUtils.showTooltip(
                        $(me.randomNumTxt),
                        'Please enter a number greater than 0',
                        'bottom',
                        'manual',
                        3000);
            $(me.randomNumTxt).val('');
            return;
        }

        // TODO show warning only if there are students assigned to groups already
        // show warning and confirm that the user would like to perform the action
        var confirmation = confirm('Random will reorganize your students into groups randomly,'
            + ' even the students that are already assigned to groups. Would you like to continue?');
        if (confirmation) {
            me.pubSub.publish('assign-random', me.studentModels, randomNum);
            $(me.randomNumTxt).val('');
        }
    }

    /**
	 * Return the student object with the given id 
	 */
    this.getStudentById = function (studentId) {
        var matchingStudent = _.find(me.studentModels, function (studentModel) {
            return studentModel.getId() === studentId;
        });
        return matchingStudent;
    }
}