var group_selection = group_selection || {};

group_selection.groupDetails = function(){
	var me = this;
	this.pubSub = PubSub;
		
	this.allStudents = [];
	this.currGroup = {};
	this.groupData = {};
	this.students = [];
	this.attributes = [];
	this.processing = false;
	
	this.groupDetails = '.group-details';
	this.groupNameClass = '.group-details .group-name';
    this.groupNameTxtClass = '.group-details .group-name-txt';
	this.groupDescriptionClass = '.group-details .group-description';
	this.groupEditImgClass = '.edit-group-icon';
	this.groupSaveImgClass = '.save-group-icon';
	this.memberListClass = '.member-list';
	
	this.studentSearchElem = "#student-search-txt";
	this.addStudentBtnElem = "#add-student-btn";
	
	this.showMoreDataBtnElem = "#show-more-data-btn";
	this.showMoreDataModalElem = "#show-more-data-modal";
	this.modalAttributesDiv = "#modal-attributes-list";
	this.attributesSaveBtnClass = ".attributes-save-btn";
	this.attributeCheckbox = ".attribute-checkbox";
	
	this.lessonPlanUploadDiv = ".group-lesson-plan-upload";
	this.lessonPlanAttachmentDiv = ".group-lesson-plan-bottom";
	this.attachmentFileInput = ".real-upload-txt";
	this.attachmentFileTxt = ".fake-upload-txt";
	this.addAttachmentBtn = ".add-attachment-btn";
	this.lessonPlanFileName = '.lesson-plan-file-name';
	this.lessonPlanRemoveIcon = '.lesson-plan-remove-icon';
	
	/**************************
     * METHODS
     **************************/
    this.init = function(allStudents, attributes){
    	me.allStudents = allStudents;
    	
    	_.each(allStudents, function(s){
    		$(me.studentSearchElem).append('<option value="' + s.id + '">' + s.name + '</option>');
    	});
    	$(me.studentSearchElem).select2( {width:'element'} );
    	$(me.showMoreDataBtnElem).click(function(event){
    		me.showMoreDataPopup();
    	});
    	
    	$(me.showMoreDataModalElem).find(me.attributesSaveBtnClass).click(function(event){
    		me.toggleAttributesVisible();
    		$(me.showMoreDataModalElem).modal('hide');
    	});
    	
    	me.attributes = attributes;
    	_.each(attributes, function(attribute){
    		var checkbox = $('<li><input class="attribute-checkbox" type="checkbox" value="' + attribute.attributeId 
    			+ '" data-displayName="' + attribute.attributeName + '"/>' 
    			+ attribute.attributeName +  "</li>");
    		$(me.modalAttributesDiv).append(checkbox);
    	});
    	
    	$(me.attachmentFileInput).change(function(event){
    		var fileName = $(me.attachmentFileInput).val();
    		$(me.attachmentFileTxt).val(fileName);
    	});
    	
    	$(me.addAttachmentBtn).click(function(event){
    		me.attachFile();
    	});
    	
    	$(me.lessonPlanRemoveIcon).click(function(event){
    		me.removeAttachment();
    	});
    	
    	$(me.addStudentBtnElem).click(function (event) {
    	    me.addSelectedStudent();
    	});

    	$(me.groupSaveImgClass).click(function (event) {
    	    me.saveGroupChanges();
    	});

    	// subscribe to events
    	me.pubSub.subscribe('show-group-details', me.viewGroupDetails);
    	me.pubSub.subscribe('remove-student', me.removeStudent);
    }
    
    /**
     *  
     */
    this.viewGroupDetails = function (group) {

        // prompt user to save any changes he/she made
        var currGroupDirty = me.currGroup.dirty;
        if (currGroupDirty) {
            var confirmation = confirm("You have unsaved changes. If you continue these changes will be lost. Continue?");
            if (!confirmation) {
                return;
            } else {
                // reset to original values
                me.currGroup.initData();
            }
        }
       
        me.currGroup = group;
        me.groupData = group.groupData;

        var groupData = group.groupData;
        $(me.groupNameClass).find("h3").html(groupData.cohortIdentifier);
        $(me.groupDescriptionClass).html(groupData.cohortDescription);

        me.students = [];
        $(me.memberListClass).empty();
        var students = group.group.students;
        _.each(students, function (studentId) {
            me.addStudent(studentId);
        });

        me.toggleLessonPlan();
        
    }
    
    /**
     *
     */
    this.addStudent = function (studentId) {
        var matchingStudent = _.find(me.allStudents, function (s) {
            return s.id === studentId;
        });

        var studentObj = new group_selection.student(matchingStudent);
        $(me.memberListClass).append(studentObj.generateTemplate());

        studentObj.init();
        var selectedStudentAttributes = me.currGroup.selectedAttributes;
        if (selectedStudentAttributes.length > 0) {
            studentObj.appendStudentAttributes(me.currGroup.selectedAttributes);
        }
        me.students.push(studentObj);
    }

    /**
     * 
     */
    this.toggleAttributesVisible = function(){
    	var selectedAttributes = [];
    	$(me.attributeCheckbox).each(function(index, elem){
    		var selected = $(elem).is(":checked");
    		if (selected){
    			var val = $(elem).val();
    			var attribute = _.find(me.attributes, function(attr){
    				return attr.attributeId === val;
    			});
    			selectedAttributes.push(attribute);
    		}
    	});
    	me.currGroup.setSelectedAttributes(selectedAttributes);
    	
    	_.each(me.students, function(student){
    		student.appendStudentAttributes(selectedAttributes);
    	});    	
    }
    
    /**
     * Show the popup for user to select which student attributes to show 
     */
    this.showMoreDataPopup = function(){    	
    	// toggle attribute checkboxes
    	$(me.attributeCheckbox).attr('checked',false);
    	
    	var selectedAttributes = me.currGroup.selectedAttributes;
    	_.each(selectedAttributes, function(attr){
    		$(me.attributeCheckbox + "[value='" + attr.attributeId + "']").attr('checked', true);
    	});
    	    	
    	$(me.showMoreDataModalElem).modal('show');
    }
    
    /**
     * Attach the file from the file input to the current group 
     */
    this.attachFile = function(){
    	var files = $(me.attachmentFileInput).prop('files');
	    var file = files[0];
		if (file !== undefined){
			me.attachedFile = file;
			
			var reader = new FileReader();

		    // Closure to capture the file information.
		     reader.onload = (function(theFile) {
		      return function(e) {
		        // Render thumbnail.
		        var result = e.target.result;
		        var contentStartIndex = result.indexOf(',');
		        var type = result.substring(0, contentStartIndex);
		        var content = result.substring(contentStartIndex+1);
		        
		        var lessonPlan = {
		        	name: file.name,
		        	type: type,
		        	content: content
		        }
		        me.currGroup.attachFile(lessonPlan);
		        
		        // show the div with the attachment
		        me.toggleLessonPlan();      
		      };
		    })(file);
		
		    // Read in the image file as a data URL.
		    reader.readAsDataURL(file);
		} else {
			me.attachedFile = null;
		}	
    }
    
    /**
     * Remove the attachment from the current group 
     */
    this.removeAttachment = function(){
        me.currGroup.removeFile();
    	me.toggleLessonPlan();
    }
    
    /**
     * Show the attached file if there is one, otherwise hide it
     */
    this.toggleLessonPlan = function(){
    	var file = me.currGroup.attachedFile;
    	if (file !== null && file !== undefined) {
	        $(me.lessonPlanFileName).attr('href', file.type + "," + file.content);
	        $(me.lessonPlanFileName).attr('download', file.name);

	        var fileName = file.name;
            // cut off filename if its too long
	        if (fileName.length > 28) {
	            fileName = file.name.substring(0, 28) + "...";
	        }
	        $(me.lessonPlanFileName).html(fileName);
		    $(me.lessonPlanAttachmentDiv).show();		    
	        $(me.lessonPlanUploadDiv).hide();	 
	    }
	    else {
	        // hide the file upload div
	        $(me.lessonPlanUploadDiv).show();	 
	        $(me.lessonPlanAttachmentDiv).hide(); 
 
	    	// reset the attachment file input
	    	$(me.attachmentFileInput).val('');
	    	$(me.attachmentFileTxt).val('');
	    }
    }

    /**
     * Adds the selected student to this group
     */
    this.addSelectedStudent = function () {
        var studentId = $(me.studentSearchElem).val();
        me.currGroup.addStudent(studentId);

        me.addStudent(studentId);

        // reset the search box
        $(me.studentSearchElem).select2('val', '');
    }

    /**
     * Remove the given student from the list of students
     */
    this.removeStudent = function (studentId) {
        // remove from list of students
        me.students = _.filter(me.students, function (student) {
            return student.studentData.id !== studentId;
        });
    }

    /**
     * Save the changes made by user
     */
    this.saveGroupChanges = function () {
        me.toggleGroupContainerProcessingState(true);
        me.currGroup.saveGroupChanges(me.saveGroupChangesSuccessHandler, me.saveGroupChangesErrorHandler);
    }

    /**
     * Callback handler for successful group save
     */
    this.saveGroupChangesSuccessHandler = function (result) {

        if (result.completedSuccessfully) {
            // Let user know the delete was not successful
            utils.uiUtils.showTooltip(
                $(me.groupNameTxtClass),
                'Group has been successfully saved.',
                'top',
                'manual',
                3000);
            me.currGroup.dirty = false;
        } else {
            me.saveGroupChangesErrorHandler(result);
        }
        me.toggleGroupContainerProcessingState(false);
    }

    /**
     * Callback handler for unsuccessful group save
     */
    this.saveGroupChangesErrorHandler = function (result) {
        // Let user know the delete was not successful
        utils.uiUtils.showTooltip(
            $(me.groupNameTxtClass),
            'Group could not be saved. Please try again later or contact your system administrator if this problem persists.',
            'top',
            'manual',
            3000);

        me.toggleGroupContainerProcessingState(false);
    }

    /**
     * If doing an ajax request, fade out the background and display spinner
     */
    this.toggleGroupContainerProcessingState = function (processing) {
        if (processing) {
            $(me.groupDetails).css('opacity', 0.5);
            $(me.groupDetails).spin();
        } else {
            $(me.groupDetails).css('opacity', 1);
            $(me.groupDetails).spin(false);
        }
        me.processing = processing;
    }
}