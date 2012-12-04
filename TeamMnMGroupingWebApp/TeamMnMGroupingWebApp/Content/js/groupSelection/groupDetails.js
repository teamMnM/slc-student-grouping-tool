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
	this.attachedFile = null;
	this.dirty = false;

	this.groupDetails = '.group-details';
	this.groupNameClass = '.group-details .group-name';
	this.groupNameTxtClass = '.group-details .group-name-txt';
	this.groupNameTxtAreaClass = '.group-name-txtarea';
	this.groupDescriptionClass = '.group-details .group-description';
	this.groupDescriptionTxtAreaClass = '.group-details .group-description-txtarea';
	this.groupEditImgClass = '.edit-group-icon';
	this.groupSaveImgClass = '.save-group-icon';
	this.memberListClass = '.member-list';
	
	this.studentSearchElem = "#student-search-txt";
	this.addStudentBtnElem = "#add-student-btn";
	this.groupAddStudentClass = ".group-add-student";

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
    	
    	$(me.groupEditImgClass).click(function (event) {
    	    me.editGroup();
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

    	$(me.groupNameTxtClass).click(function (event) {
    	    me.makeGroupNameEditable();
    	});

    	$(me.groupDescriptionClass).click(function (event) {
    	    me.makeGroupDescriptionEditable();
    	});

    	// subscribe to events
    	me.pubSub.subscribe('show-group-details', me.viewGroupDetails);
    	me.pubSub.subscribe('remove-student', me.removeStudent);
    	me.pubSub.subscribe('group-deleted', me.hideContent);
    }
    
    /**
     *  
     */
    this.viewGroupDetails = function (group) {

        // group-details pane is hidden by default, 
        // we only show when a user clicks on a group for details
        if ($(me.groupDetails).css('display') === 'none') {
            $(me.groupDetails).show();
        }

        // prompt user to save any changes he/she made
        var dirty = me.dirty;
        if (dirty) {
            var confirmation = confirm("You have unsaved changes. If you continue these changes will be lost. Continue?");
            if (!confirmation) {
                return;
            } else {
                // reset to original values
                me.currGroup.initData();
            }
        }
       
        me.currGroup = group;
        me.currGroup.showArrow();
        me.groupData = group.groupData;

        var groupData = group.groupData;
        $(me.groupNameTxtClass).html(groupData.cohortIdentifier);

        var groupDescription = groupData.cohortDescription;
        if (groupDescription === null || utils.uiUtils.textIsEmpty(groupDescription)) {
            groupDescription = "<i>[add description here]</i>";
        };

        $(me.groupDescriptionClass).html(groupDescription);

        me.students = [];
        $(me.memberListClass).empty();
        var students = group.students;
        _.each(students, function (studentId) {
            me.addStudent(studentId);
        });

        me.attachedFile = me.currGroup.attachedFile;
        me.toggleLessonPlan();

        // only show save btn if there has been changes
        me.toggleDirty(false);
        
    }
    
    /**
     * Return boolean indicating whether the given student was added
     */
    this.addStudent = function (studentId) {

        // check if student has already been added to this group
        var existingStudent = _.find(me.students, function (s) {
            return s.studentData.id === studentId;
        });

        // exit out of func if student has already been added
        if (existingStudent !== undefined && existingStudent !== null) {
            utils.uiUtils.showTooltip(
               $(me.groupAddStudentClass),
               'Student is already in this group.',
               'top',
               'manual',
               3000);
            return false;
        }

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
        return true;
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
    	me.toggleDirty(true);
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
		        me.attachedFile = lessonPlan;
		        
		        // show the div with the attachment
		        me.toggleLessonPlan();
		        me.toggleDirty(true);
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
    this.removeAttachment = function () {
        me.attachedFile = null;
        me.toggleDirty(true);
    	me.toggleLessonPlan();
    }
    
    /**
     * Show the attached file if there is one, otherwise hide it
     */
    this.toggleLessonPlan = function(){
        var file = me.attachedFile;
    	if (file !== null && file !== undefined) {
	        $(me.lessonPlanFileName).attr('href', file.type + "," + file.content);
	        $(me.lessonPlanFileName).attr('download', file.name);

	        var fileName = file.name;
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

        var added = me.addStudent(studentId);
        me.toggleDirty(added);
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
        me.toggleDirty(true);
    }

    /**
     * Save the changes made by user
     */
    this.saveGroupChanges = function () {

        // hack to save name/description if they are being edited
        setTimeout(function () {

            var groupName = $(me.groupNameTxtClass).html();
            me.currGroup.setName(groupName);

            var groupDescription = $(me.groupDescriptionClass).html();
            // make sure we are not just assigning the default text
            if (groupDescription === "<i>[add description here]</i>") {
                groupDescription = "";
            }
            me.currGroup.setDescription(groupDescription);

            me.toggleGroupContainerProcessingState(true);
            me.currGroup.attachedFile = me.attachedFile;
            me.currGroup.saveGroupChanges(me.saveGroupChangesSuccessHandler, me.saveGroupChangesErrorHandler);
        }, 100);
    }

    /**
     * Callback handler for successful group save
     */
    this.saveGroupChangesSuccessHandler = function (result) {

        if (result.completedSuccessfully) {
            // Let user know the delete was successful
            utils.uiUtils.showTooltip(
                $(me.groupSaveImgClass),
                'Group has been successfully saved.',
                'right',
                'manual',
                3000);
            me.toggleDirty(false);

            if (me.attachedFile !== null) {
                me.currGroup.attachFile(me.attachedFile);
            } else if (me.currGroup.attachedFile !== null) {
                // if currGroup has file, but me.attachedFile is null it means we removed the group's file
                me.currGroup.removeFile();
            }
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
            $(me.groupSaveImgClass),
            'Group could not be saved. Please try again later or contact your system administrator if this problem persists.',
            'right',
            'manual',
            4000);

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

    /**
     * Go to multiple groups edit page to edit this group
     */
    this.editGroup = function () {
        var groupId = me.groupData.id;
        window.location = 'MultipleGroupsEdit?selGroups=' + groupId;
    }

    /**
     * Make the group name label editable, turns it into a textbox
     */
    this.makeGroupNameEditable = function () {
        var groupName = $(me.groupNameTxtClass).html();
        $(me.groupNameTxtClass).hide();
        $(me.groupNameTxtAreaClass)
            .val(groupName)
            .show()
            .focus();

        $(me.groupNameTxtAreaClass).unbind('blur');
        $(me.groupNameTxtAreaClass).blur(function (event) {
            setTimeout(function () {
                me.saveGroupName();
            }, 50);
        });
    }

    /**
     * Save the new group name
     */
    this.saveGroupName = function () {
        var newGroupName = $(me.groupNameTxtAreaClass).val();

        // if no input then set default name
        if (!/\S/.test(newGroupName)) {
            newGroupName = 'New Group';
        }

        $(me.groupNameTxtClass).html(newGroupName);
        $(me.groupNameTxtClass).show();
        $(me.groupNameTxtAreaClass).hide();
        me.toggleDirty(true);
    }

    /**
	 * Make the group description text editable, turns it into a textarea
	 */
    this.makeGroupDescriptionEditable = function () {
        var groupDescription = $(me.groupDescriptionClass).text();
        if (groupDescription === '[add description here]') {
            groupDescription = "";
        }

        $(me.groupDescriptionClass).hide();
        $(me.groupDescriptionTxtAreaClass)
            .val(groupDescription)
            .show()
            .focus();

        $(me.groupDescriptionTxtAreaClass).unbind('focusout');
        $(me.groupDescriptionTxtAreaClass).focusout(function (event) {
            me.saveGroupDescription();
        });
    }

    /**
     * Save the group description
     */
    this.saveGroupDescription = function () {
        var newGroupDescription = $(me.groupDescriptionTxtAreaClass).val();

        // make sure that there is a description
        if (!utils.uiUtils.textIsEmpty(newGroupDescription)) {
            $(me.groupDescriptionClass).html(newGroupDescription);
            me.toggleDirty(true);
        }

        $(me.groupDescriptionClass).show();
        $(me.groupDescriptionTxtAreaClass).hide();
    }

    /**
     *
     */
    this.toggleDirty = function (dirty) {
        me.dirty = dirty;
        if (dirty) {
            $(me.groupSaveImgClass).show();
        } else {
            $(me.groupSaveImgClass).hide();
        }
    }

    /**
     * Hide the group details panel
     */
    this.hideContent = function () {
        $(me.groupDetails).hide();
    }
}
