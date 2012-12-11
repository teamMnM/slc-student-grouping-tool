var student_grouping = student_grouping || {};

student_grouping.studentsList = function(){
	var me = this;
	this.pubSub = PubSub;
	
	this.students = [];
	this.studentListElem = '#studentList';
	
	this.selectAllBtn = '#select-all-btn';
	this.randomBtn = '#random-btn';
	this.randomNumTxt = '#random-num-txt';
	this.studentSearchBox = '#txtStudentSearchBox';
		
	// keep track of whether user has selected all
	this.allSelected = false;
	
	/**************************
     * METHODS
     **************************/
	this.init = function(students){		
					
		// add all students to the list
		for (var i = 0; i < students.length; i++){
		    var studentDisplayObject = students[i];
			
		    var student = new student_grouping.student(studentDisplayObject);
			$(this.studentListElem).append(student.generateTemplate());			
			student.init();	
			
			this.students.push(student);
		}
		
		// add event handler for filter student list using search box
		$(this.studentSearchBox).keyup(function(){
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
		
		$(this.selectAllBtn).click(function(event){
			me.selectAllStudents();
		});	
					
		$(this.randomBtn).click(function(event){
			me.assignRandom();
		});
					
		// TODO add description
		this.pubSub.subscribe('filter-student-list', function(){
			me.filterStudentList();
		});
		
		this.pubSub.subscribe('student-selection-changed', function(studentId){
			// TODO add method
		});

		$('#studentListDiv .box-wrap').antiscroll();
				
	}	
	
	/**
	 * Repopulate the list of students with given list 
	 */
	this.changeSelectableList = function(listStudentData, changeEventHandler){
		var options = [];
		_.each(listStudentData, function(studentData){
			return options.push(
				{ id : studentData.id, text : studentData.name });
		});
		
		$(this.studentSearchBox).select2('destroy');
		$(this.studentSearchBox).select2(
			{ 
				data : options, 
				width : 'element'
			});
		$(this.studentSearchBox).on('change', changeEventHandler);
	}
	
	/**
	 *  
	 */
	this.filterStudentList = function(){				
		// TODO refactor dependency on filter component
		var filteredStudents = student_grouping.filterComponent.applyFilters(this.students);
		_.each(this.students, function(studentLi){
			var filteredStudent = _.find(filteredStudents, function(s){
				return s.studentData.id === studentLi.studentData.id;
			});
			studentLi.toggleVisible(filteredStudent !== undefined);
		});

	    // if the deselect all btn has been toggled, change it back to select all
		$(this.selectAllBtn).html('select all');
	}
	
	/**
	 * Select all students in the list 
	 */
	this.selectAllStudents = function(){		
		_.each(this.students, function(student){
		    if (student.visible) {
		        student.toggleStudentSelection(!me.allSelected);
		    }
		});
		
		this.allSelected = !this.allSelected;
		
		if (this.allSelected){
			$(this.selectAllBtn).html('deselect all');
		} else {
			$(this.selectAllBtn).html('select all');
		}
	}
	
	/**
	 * Randomly organizes all students into groups (reassigns students that are already
     * assigned to groups)
	 */
	this.assignRandom = function () {

	    var randomNum = $(this.randomNumTxt).val().trim();
	    if (isNaN(randomNum) || randomNum === '' || parseInt(randomNum) <= 0) {
	        utils.uiUtils.showTooltip(
                        $(this.randomNumTxt),
                        'Please enter a valid number',
                        'bottom',
                        'manual',
                        3000);
	        $(this.randomNumTxt).val('');
	        return;
	    }

        // TODO show warning only if there are students assigned to groups already
        // show warning and confirm that the user would like to perform the action
	    var confirmation = confirm('Random will reorganize your students into groups randomly,' 
            + ' even the students that are already assigned to groups. Would you like to continue?');
	    if (confirmation) {
	        var me = this;
	        this.pubSub.publish('assign-random', this.students, randomNum);
	        $(this.randomNumTxt).val('');	        
	    }
	}
	
	/**
	 * Return the student object with the given id 
	 */
	this.getStudentById = function(studentId){
		var matchingStudent = _.find(this.students, function(student){
			return student.studentData.id === studentId;
		});
		return matchingStudent;
	}
}
