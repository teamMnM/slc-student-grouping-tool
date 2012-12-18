var group_selection = group_selection || {};

group_selection.group = function(groupData){
	var me = this;
	this.pubSub = PubSub;
	
	this.dirty = false;
	this.group = groupData;
	this.groupData = groupData.cohort;
	this.originalStudents = groupData.students;
	this.students = [];
	this.selectedAttributes = [];
	this.attachedFile = null;

	this.groupContainerId = '';
	this.groupContainerClass = '.group-container';
	this.groupTitleClass = '.group-title';
	this.groupModifiedTimestampClass = '.group-modified-timestamp';
	this.groupDescriptionClass = '.group-description';
	this.groupToggleInfoClass = '.group-toggle-info';		
    
	this.groupDownloadLinkClass = '.group-download-link';
	this.groupAttachmentLinkClass = '.group-attachment-link';
	this.groupAttachmentIconClass = '.group-attachment-icon';
	this.groupAttachmentPrintClass = '.group-print-icon';
	this.groupDeleteIconClass = '.group-delete-icon';
	this.groupTemplate = '<div class="group-list-item">' + 
									'<div class="group-checkbox"><input type="checkbox" class="group-checkbox"/></div>' +
									'<div class="group-container">' + 
										'<div>' +
											'<span class="group-title"></span>' +
                                            '<a class="group-download-link" href="#"><img src="/Content/img/download-icon.png" class="group-icon"></img></a>' +
											'<a class="group-attachment-link" href="#"><img src="/Content/img/attachment-icon.png" class="group-icon group-attachment-icon"></img></a>' +
											'<img src="/Content/img/printer-icon.png" class="group-icon group-print-icon"></img>' +
											'<img src="/Content/img/trash-icon.png" class="group-icon group-delete-icon"></img>' +
											'<i class="group-modified-timestamp">Last modified: </i>' + 	
										'</div>' + 
										'<div>' + 
											'<p class="group-description"></p>' +
											'<a href="#" class="group-toggle-info"></a>' +
										'</div>'
									'</div>' +
								'</div>';    

	/**************************
     * METHODS
     **************************/
    this.init = function(){
    	me.groupContainerId = "#" + me.groupData.id; 
    	$(me.groupContainerId).click(function (event) {
    	    me.groupSelected();
    	});

    	$(me.groupContainerId).find(me.groupDeleteIconClass).click(function (event) {
    	    me.deleteGroup();
    	});

    	$(me.groupContainerId).find(me.groupAttachmentPrintClass).click(function (event) {
    	    me.printGroup();
    	});

    	$(me.groupContainerId).find(me.groupDownloadLinkClass).click(function (event) {
    	    me.downloadGroup();
    	});

    	me.pubSub.subscribe('remove-student', function (studentId) {
    	    me.removeStudent(studentId);
    	});

    	me.initData();
    }							
	
    /**
     *
     */
    this.initData = function () {
        me.students = [];
        // make copy of student list for modifying
        _.each(me.originalStudents, function (student) {
            me.students.push(student);
        });

        me.selectedAttributes = [];
        var dataElements = me.group.custom.dataElements;
        if (dataElements !== undefined && dataElements !== null && dataElements.length > 0) {
            _.each(dataElements, function (dataElement) {
                me.selectedAttributes.push(dataElement);
            });
        }

        // if a file has been uploaded for the group, set as attached file
        var lessonPlan = me.group.custom.lessonPlan;
        if (lessonPlan !== undefined && lessonPlan !== null) {
            me.attachedFile = lessonPlan;
        }

        me.showFileAttachment();
        me.dirty = false;
    }

	/**
	 * Return the HTML content for this object 
	 */										 
	 this.generateTemplate = function(){
	 	var custom = me.group.custom;
	 	var groupData = me.groupData;
	 	var template = $(me.groupTemplate);
	 	
	 	$(template).attr('id', groupData.id);
	 	$(template).find(me.groupTitleClass).html(groupData.cohortIdentifier);

	 	var lastModifiedDate = new Date(parseInt(custom.lastModifiedDate.replace('/Date(', '').replace(')/', '')));
	 	var lastModifiedDateStr = lastModifiedDate.toFormat('MM/DD/YYYY HH:MI PP');
	 	$(template).find(me.groupModifiedTimestampClass).html('Last modified: ' + lastModifiedDateStr);
	 	$(template).find(me.groupDescriptionClass).html(groupData.cohortDescription);

	 	return template;	 	
	 }


    /**
     * TODO implement better solution for cutting off long file names
	 * Show the attached file
	 */
	 this.showFileAttachment = function () {
	     var file = me.attachedFile;
	     if (file !== null && file !== undefined) {
	         $(me.groupContainerId).find(me.groupAttachmentLinkClass).attr('href', file.type + "," + file.content);
	         $(me.groupContainerId).find(me.groupAttachmentLinkClass).attr('download', file.name);

	         $(me.groupContainerId).find(me.groupAttachmentLinkClass).show();
	     } else {
	         $(me.groupContainerId).find(me.groupAttachmentLinkClass).hide();
	     }
	 }
	
	 /**
	  * Set the group's selected attributes to be the given attributes 
	  */		
     this.setSelectedAttributes	= function(attributes){
     	me.selectedAttributes = [];
     	_.each(attributes, function(attr){
     		me.selectedAttributes.push(attr);
     	});
     	me.dirty = true;
     }		
     
     /**
	 * Mark this group as dirty so that changes are saved back to server 
	 */
	this.markDirty = function(){
		this.dirty = true;
	}
	
	/**
	 * Save the changes for this group
	 */		
	this.saveGroupChanges = function (successHandler, errorHandler) {
	    // for some reason, the ajax callback function doesn't recognize successHandler and errorHandler unless we 
        // use closure to make it part of this group's scope
	    me.successHandler = successHandler;
	    me.errorHandler = errorHandler;

	    var cohortActionObject = this.prepareGroupForSaving();

	    // negative ids represent new groups
	    var method = 'CreateGroup';
	    var successHandler = null;
	    var errorHandler = null;
	    if (parseInt(this.groupData.id) < 0) {
	        cohortActionObject.cohort.id = null;
	    } else {
	        method = 'UpdateGroup';
	    }

	    $.ajax({
	        type: 'POST',
	        url: method,
	        contentType: 'application/json',
	        data: JSON.stringify(cohortActionObject),
	        success: function (result) {	            

                // add the newly created students
	            var newStudents = cohortActionObject.studentsToCreate;

	            if (!result.completedSuccessfully){
	                // determine which students could not be created
	                var failToCreateAssociations = result.failToCreateAssociations;

	                newStudents = _.filter(newStudents, function(newStudent){
	                    var failed = _.find(failedToCreateAssociations, function(failedAssociation){
	                        return failedAssociation.data === newStudent;
	                    });
	                    return failed === undefined;
	                });
	            }
	            
	            _.each(newStudents, function(newStudent){
	                me.originalStudents.push(newStudent);
	            });

                // remove deleted students
                var studentsToDelete = cohortActionObject.studentsToDelete;
                _.each(studentsToDelete, function (studentToDel) {
                    me.originalStudents = _.filter(me.originalStudents, function (originalStudent) {
                        return originalStudent !== studentToDel;
                    });
                });
                
                if (result.completedSuccessfully) {
                    // update the group's timestamp, if successful save
                    var currTimestamp = new Date();
                    me.group.custom.lastModifiedDate = currTimestamp.getTime().toString();
                    $(me.groupContainerId).find(me.groupModifiedTimestampClass).html(currTimestamp.toFormat('MM/DD/YYYY HH:MI PP'))
                    me.pubSub.publish('reorder-group', me.group);
                }

                me.successHandler(result);
	        },
	        error: me.errorHandler
	    });
	 }
	 
	 /**
	  * Delete this group 
	  */
	 this.deleteGroup = function(){
	    var groupId = me.groupData.id;
	        var confirmation = confirm('Are you sure you want to delete the group: ' + me.groupData.cohortIdentifier);
	        if (confirmation) {
	            // make sure we are not deleting newly created, unsaved groups
	            if (groupId < 0) {
	                // Warn the user they are deleting a new, unsaved group
	                utils.uiUtils.showTooltip(
                        $(me.groupContainerId).find(me.groupTitleClass),
                        'Cannot delete this unsaved new group.',
                        'top',
                        'manual',
                        3000);
	            } else {
	                me.toggleGroupContainerProcessingState(true);
	                $.ajax({
	                    type: 'POST',
	                    url: 'DeleteGroup?id=' + groupId,
	                    contentType: 'application/json',
	                    success: function (result) {
	                        if (result.completedSuccessfully) {
	                            me.deleteGroupSuccessHandler(result);
	                        } else {
	                            me.deleteGroupErrorHandler(result);
	                        }
	                    },
	                    error: function (result) {
	                        deleteGroupErrorHandler(result);
	                    }
	                });
	            }
	        }
	 }
	 
	 /**
     * Creates JSON object to send back to server for saving changes
     */
	this.prepareGroupForSaving = function () {
	    var newStudents = _.filter(this.students, function (student) {
	        var matchingStudent = _.find(me.originalStudents, function (origStudentId) {
	            return origStudentId === student;
	        });
	        return matchingStudent === undefined;
	    });
	    // remove blanks
	    newStudents = _.filter(newStudents, function (student) {
	        return student !== "";
	    });

	    var studentsToDelete = _.filter(this.originalStudents, function (origStudentId) {
	        var matchingStudent = _.find(me.students, function (student) {
	            return origStudentId === student;
	        });
	        return matchingStudent === undefined;
	    });
	    // remove blanks
	    studentsToDelete = _.filter(studentsToDelete, function (student) {
	        return student !== "";
	    });

        // if negative, then it is a new group so it doesn't have an id
	    var id = parseInt(this.groupData.id) < 0 ?
             null : this.groupData.id;
        
	    var cohortActionObject = {
	        cohort: {
	            id: id,
	            cohortDescription: this.groupData.cohortDescription,
	            cohortIdentifier: this.groupData.cohortIdentifier
	        },
	        custom: { dataElements: me.selectedAttributes, lessonPlan: me.attachedFile },
	        studentsToDelete: studentsToDelete !== null ? studentsToDelete : [],
	        studentsToCreate: newStudents !== null ? newStudents : []
	    }

	    return cohortActionObject;
	}			
	
	/**
     * Callback handler for successful delete
     */
	this.deleteGroupSuccessHandler = function () {
	    me.pubSub.publish('group-deleted', me.groupData.id);

	    // Let user know the delete was successful
	    utils.uiUtils.showTooltip(
            $(me.groupContainerId).find(me.groupTitleClass),
            'Group has been successfully deleted.',
            'top',
            'manual',
            2000);
	    setTimeout(function () {
	        me.remove();
	    }, 2000);
	}

    /**
     * Removes this group's DOM element
     */
	this.remove = function () {
	    $(me.groupContainerId).remove();
	}

    /**
     * Callback handler for unsuccessful delete
     */
	this.deleteGroupErrorHandler = function () {
	    // Let user know the delete was not successful
	    utils.uiUtils.showTooltip(
            $(me.groupContainerId).find(me.groupNameLblClass),
            'Group could not be deleted. Please try again later or contact your system administrator.',
            'top',
            'manual',
            3000);
	    me.toggleGroupContainerProcessingState(false);
	}

    /**
     * Adds the given student to this group's list of students
     */
	this.addStudent = function (studentId) {
	    me.students.push(studentId);
	    me.dirty = true;
	}

    /**
     * Remove the given student from this group's list of students
     */
	this.removeStudent = function (studentId) {

        // check if student is in group
	    var student = _.find(me.students, function (id) {
	        return id === studentId;
	    });

	    if (student !== undefined && student !== null) {
	        // remove from list of students
	        me.students = _.filter(me.students, function (id) {
	            return id !== studentId;
	        });
	        me.dirty = true;
	    }
	}

    /**
     * If doing an ajax request, fade out the background and display spinner
     */
	this.toggleGroupContainerProcessingState = function (processing) {
	    if (processing) {
	        $(me.groupContainerId).css('opacity', 0.5);
	        $(me.groupContainerId).spin();
	    } else {
	        $(me.groupContainerId).css('opacity', 1);
	        $(me.groupContainerId).spin(false);
	    }
	    me.processing = processing;
	}

    /**
     * Add the given file attachment to this group
     */
	this.attachFile = function (file) {
	    me.attachedFile = file;
	    me.showFileAttachment();
	}

    /**
     * Remove the file attached to this group
     */
	this.removeFile = function () {
	    me.attachedFile = null;
	    me.showFileAttachment();
	}

    /**
     * Hide/show the group
     */
	this.toggleVisible = function (visible) {
	    if (visible) {
	        $(me.groupContainerId).show();
	    } else {
	        $(me.groupContainerId).hide();
	    }
	}

    /**
     * Returns true if the group is selected
     */
	this.isSelected = function () {
	    var checkbox = $(me.groupContainerId).find('input.group-checkbox');
	    return $(checkbox).is(':checked');
	}

    /**
     * Update the group's name
     */
	this.setName = function (newName) {
	    me.groupData.cohortIdentifier = newName;
	    $(me.groupContainerId).find(me.groupTitleClass).html(newName);
	}

    /**
     * Update the group's description
     */
	this.setDescription = function (newGroupDescription) {
	    me.groupData.cohortDescription = newGroupDescription;
	    $(me.groupContainerId).find(me.groupDescriptionClass).html(newGroupDescription);
	}

    /**
     * Handle this group's selected event
     */
	this.groupSelected = function () {
	    me.pubSub.publish('show-group-details', me);

	    // apply selected styling
	    me.applySelectedStyle();
	}

    /**
     *
     */
	this.applySelectedStyle = function () {
	    $(me.groupContainerClass).css('background-color', 'white');
	    $(me.groupContainerId).find(me.groupContainerClass).css('background-color', '#F2F2F2');
	}

    /**
     * Get the group widget's absolute top position
     */
	this.getOffsetTop = function () {
	    var offset = $(me.groupContainerId).offset();
	    return offset.top;
	}

    /**
     *
     */
	this.printGroup = function () {
	    var div = me.generatePrintableHtml();
	    utils.printUtils.print($(div).html());
    }

    /**
     * Download the contents of this group to a text file
     */
	this.downloadGroup = function () {
	    window.open('DownloadGroup?id=' + me.groupData.id);
	}

    /**
     *
     */
	this.generatePrintableHtml = function () {
	    var div = $("<div>");
	    $(div).append("<h2>" + me.groupData.cohortIdentifier + "</h2>");
	    $(div).append("<p><i>" + me.groupData.cohortDescription + "</i></p>");

	    var students = me.students;
	    if (students.length > 0) {
	        var studentList = $("<ul style='list-style:none'>");
	        var allStudents = group_selection.groupDetailsComponent.allStudents;
	        _.each(students, function (studentId) {
	            var student = _.find(allStudents, function (s) {
	                return s.id === studentId;
	            });
	            $(studentList).append("<li>" + student.name + "</li>");
	        });
	        $(div).append(studentList);
	    } else {
	        $(div).append("<div><i>[no students]</i></div>");
	    }
	    return div;
	}
}
