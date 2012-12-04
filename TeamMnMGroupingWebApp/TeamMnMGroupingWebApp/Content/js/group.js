var student_grouping = student_grouping || {};

student_grouping.group = function(groupData){
	var me = this;
	this.pubSub = PubSub;
	
	this.dirty = false;
	this.processing = false;    
	this.group = groupData;
	this.groupData = groupData.cohort;
	this.originalStudents = groupData.students;
	this.students = [];
	this.color = null;

	this.groupContainerId = '';
    this.groupTopContainerClass = '.group-top-container';
	this.groupContainerClass = '.group-container';
	this.groupClass = '.group';
	this.groupNameClass = '.group-name';
	this.groupNameLblClass = '.group-name-lbl';
	this.groupNameTxtClass = '.group-name-txt';
	
	this.groupControlsClass = '.group-controls';
	this.delGroupBtnClass = '.del-group-btn';
	this.saveGroupBtnClass = '.save-group-btn';
	this.expColGroupBtnClass = '.exp-col-group-btn';
	this.expandedClass = 'expanded'; // not using as selector so don't need the .
	this.collapsedClass = 'collapsed'; // not using as selector so don't need the .
	this.collapsed = false;
	
	this.addDataDivClass = '.add-data-div';
	this.addDataBtnClass = '.add-data-button';
	
	this.selectedAttributes = [];
	
	this.groupCloseBtnClass = '.group-close-btn';
			
	this.groupPopoverClass= '.group-popover';
	this.studentPopoverElem = '.student-data-popover';	
	
	this.groupInfoBtnClass = '.group-info-btn';
	this.groupInfoPopoverElem = '.group-description-popover';
	this.groupDescriptionTxtElem = '.group-description-text';
	this.groupDescriptionTxtAreaElem = '.group-description-textarea';
	
	this.groupAttachmentImgClass = '.group-attachment-img';
	this.groupNumStudentsBadgeClass = '.group-num-students-badge';
	this.groupAttachmentPopoverElem = '.group-attachment-popover';
	this.groupAttachmentPopoverFileInput = '.real-upload-txt';
	this.groupAttachmentPopoverFileTxt = '.fake-upload-txt';
	this.groupAttachmentPopoverDoneBtnElem = '.attachment-done-btn';
	this.groupAttachmentDivClass = '.group-file-attachment';
	this.groupAttachmentNameClass = '.file-attachment-name';
	this.groupAttachmentDelImgClass = '.del-attachment-img';
	this.attachedFile = null;
	
	this.groupUnsavedChangesModalElem = '#group-unsaved-changes-modal';
	this.groupUnsavedChangesGroupName = '#group-unsaved-changes-group-name';
	this.groupUnsavedChangesConfirmBtnElem = '#group-unsaved-changes-confirm-btn';
	/**
	 * HTML template to be rendered to screen 
	 */
	this.groupContainerTemplate = '<div class="group-top-container">' +
                                    '<div class="group-container disable-select">' +
									    '<div class="group-controls">' +
										    '<button class="btn btn-link del-group-btn">delete</button>' +
										    '<button class="btn btn-link save-group-btn">save</button>' +
										    '<button class="btn btn-link exp-col-group-btn">collapse</button>' +
									    '</div>' +
									    '<div class="group-name">' +
										    '<div class="group-name-lbl"></div>' +
										    '<textarea class="group-name-txt" style="display:none; overflow:hidden; resize:none; width:10em; height:1em; background-color:transparent; text-align:center; color:white; border-color:transparent"/></div>' +
									    '<img class="hide-button group-close-btn" src="/Content/img/group-close-icon.png"></img>' +
									    '<img class="hide-button group-info-btn" src="/Content/img/group-info-icon.png"></img>' +
                                        '<div class="group"></div>' +
									    '<div style="text-align:center">' +
										    '<img class="group-attachment-img" src="/Content/img/attachment-icon.png"/>' +
										    '<span class="badge group-num-students-badge"></span>' +
									    '</div>' +
									    '<div class="add-data-div">' +
										    '<button class="add-data-button btn btn-link">add data</button>' +
									    '</div>' +
									     '<div class="group-file-attachment">' +
										     '<a class="file-attachment-name"/>' +
										     '<img class="del-attachment-img" src="/Content/img/trash-icon.png"/>' +
									     '</div>' +
								     '</div>' +
                                     '<div class="group-description-popover group-popover" data-groupContainerId="-1" style="display:none">' +
			                                     '<strong>Description:</strong>' +
			                                     '<div class="group-description-text">' +
				                                     '&nbsp;' +
                                                 '</div>' +
                                                 '<textarea class="group-description-textarea"></textarea>' +
                                     '</div>' +
                                     '<div class="student-data-popover group-popover" data-groupContainerId="-1" style="display: none">' +
			                            'select what you want to show' +
			                            '<ul class="student-elements-list">' +
			                            '</ul>' +
		                             '</div>' +
                                     '<div class="group-attachment-popover" data-groupContainerId="-1" style="display: none">' +
			                            '<div class="attachment-top-bar">' +
				                            '<span>choose a file</span>	' +
				                            '<button class="attachment-done-btn">done</button>' +
			                            '</div>' +
			                            '<div>' +
				                            '<div class="fakeupload">' +
					                            '<input type="text" class="fake-upload-txt"/>' +
					                            '<img src="/Content/img/attachment-icon.png" class="attachment-img"/>' +
					                            '<input type="file" class="real-upload-txt" class="realupload"/>' +
				                            '</div>' +
			                            '</div>' +
		                            '</div>' +
			                       '</div>';

	this.droppedElemClass = '.dropped-elem';
	this.studentAttributesClass = '.student-attributes';					
	this.fullProfileLinkClass = '.student-full-profile a';
	this.droppedElemTemplate = "<div data-studentId='' class='dropped-elem'>" +					
									"<img class='del-button' src='/Content/img/student-close-icon.png'></img>" +
									'<div class="student-icon-div"><img class="student-icon" src="/Content/img/student-icon-male.png"/></div>' +
									"<div class='student-name'></div>" + 
									"<div class='student-attributes'></div>" +
                                    "<div class='student-full-profile'><a href='#'>full profile</a></div>" +
								"</div>";				
								
	/**************************
     * METHODS
     **************************/
	
	this.init = function(dataElements) {
		var me = this;
		var groupContainer = $("#gc" + this.groupData.id); 
		$(groupContainer).find(this.addDataBtnClass).click(function (event) {
		    if (!me.processing) {
		        me.showStudentDataPopup();
		    }
		});
		
		$(groupContainer).find(this.groupInfoBtnClass).click(function (event) {
		    if (!me.processing) {
		        me.showMoreInfoPopup();
		    }
		});
		
		$(groupContainer).find(this.groupAttachmentImgClass).click(function(event){
		    if (!me.processing) {
		        me.showAttachmentPopover();
		    }
		});
		
		$(groupContainer).find(this.groupAttachmentDelImgClass).click(function(event){
		    if (!me.processing) {
		        me.deleteAttachment();
		    }
		});
		
		$(groupContainer).find(this.expColGroupBtnClass).click(function(event){		    
			me.toggleStudentState();
		});
		
		$(groupContainer).find(this.groupCloseBtnClass).click(function(event){
		    if (!me.processing) {
		        me.closeGroup();
		    }
		});
		
		$(groupContainer).find(this.delGroupBtnClass).click(function(event){
		    if (!me.processing) {
		        me.deleteGroup();
		    }
		});
		
		$(groupContainer).find(this.groupNameLblClass).click(function(event){
		    if (!me.processing) {
		        me.makeGroupNameEditable();
		    }
		});
		
		$(groupContainer).find(this.saveGroupBtnClass).click(function (event) {
		    if (!me.processing && me.dirty) {
		        me.saveGroupChanges();
		    } else if (!me.dirty) {
		        // Let user know the created was successful
		        utils.uiUtils.showTooltip(
                    $(me.groupContainerId).find(me.groupNameLblClass),
                    'This group does not have unsaved changes.',
                    'top',
                    'manual',
                    3000);
		    }
		});

		this.groupContainerId = groupContainer;
		this.updateNumStudentsBadge();

        // add the pre-defined data elements to the popover
		_.each(dataElements, function (dataElement) {
		    var dataElem = $("<li><input class='cbox-student-attribute' type='checkbox'"
                + "value='" + dataElement.attributeId + "' data-displayName='"
                + dataElement.attributeName + "'/>" + dataElement.attributeName
                + "</li>");
		    $(me.groupContainerId).find(".student-data-popover .student-elements-list")
                .append(dataElem);
		});

	    // load the selected attributes
		var custom = this.group.custom;
		if (custom !== null && custom !== undefined) {
		    var selectedAttributes = custom.dataElements;
		    _.each(selectedAttributes, function (selectedAttribute) {
		        me.selectedAttributes.push(selectedAttribute);
		    });
		}

		// render students assigned to this group
		var studentIds = this.originalStudents;
		_.each(studentIds, function(studentId){
			
			// TODO refactor dependency on studentsList
			var student = student_grouping.studentsListComponent.getStudentById(studentId);
			if (student !== undefined){			
				me.assignStudentToGroup(student);	
			}
		});		
		
	    // load attached lesson plan
		if (this.group.custom !== null && this.group.custom.lessonPlan !== null) {
		    this.attachedFile = this.group.custom.lessonPlan;
		    this.showFileAttachment();
		}

	    // disable all group actions while save all is going on
		me.pubSub.subscribe('save-all-groups', function () {
		    me.processing = true;
		});

	    // re-enable all group actions once save all is done
		me.pubSub.subscribe('save-all-completed', function () {
		    me.processing = false;
		    me.dirty = false;
		});
	};
		
	/**
	 * Add the given student html elem to the given group
	 * @param {Object} student
	 */
	this.assignStudentToGroup = function(student){
		var studentId = student.studentData.id;	
		var groupId = this.groupData.id;
			
		// check if elem is in group already		
		if ($("#" + groupId + " #dr-" + studentId).length === 0){	
			var droppedElem = this.createDroppedElem(student.studentData);
			$("#" + groupId).append(droppedElem);
			
			// make it draggable to another group
			$(droppedElem).draggable({
				drag: function(event, ui){
					
					// TODO refactor shortcut to groupList component
					student_grouping.groupsListComponent.currGrp = null;
				},
				revert:"invalid",
				"helper":"clone", 
				"opacity":0.7 
			});
			
			// add student to list of students
			this.students.push(studentId);
			student.addGroupIndicator(this.groupData.id, this.color.background);

		    // make selected student attributes visible (only if group is not collapsed)
			if (!me.collapsed) {
			    this.toggleStudentAttributeVisibility(this.selectedAttributes);
			}

            // update student count label
			this.updateNumStudentsBadge();
		}
	}
	
	
	/**
	 * Removes the selected student from its group  
	 * @param {String} studentId
	 */
	this.removeStudent = function(studentId){		
		var studentElem = $(this.groupContainerId).find('.dropped-elem[data-studentId="'+ studentId + '"]');
		
		// find student object from list 
		var student = _.find(this.students, function(id){
			return id === studentId;
		});
		
        // tell student it has been removed from this group
		this.pubSub.publish('remove-group-indicator', studentId, this.groupData.id);
		
		// remove from list of students
		this.students = _.filter(this.students, function(id){
			return id !== studentId;
		});
		
		// remove the student inside the group
		$(studentElem).remove();
		 
		this.markDirty();

	    // update student count label
		this.updateNumStudentsBadge();
	}
	
	/**
	 * Returns true if the group already contains the student with the given id 
	 */
	this.hasStudent = function(studentId){
		var student = _.find(this.students, function(id){
			return id === studentId;
		});
		return student !== undefined;
	}

	
	/**
	 * TODO externalize element selectors and remove manual width of dropped elem
     * Create group indicator for the given student
	 * @param {Object} student
	 */
	this.createDroppedElem = function(student){
		var me = this;
		var elemDiv = $(this.droppedElemTemplate);
		$(elemDiv).attr('id', 'dr-' + student.id);
		$(elemDiv).attr('data-studentId', student.id);
		$(elemDiv).find('.student-name').html(student.name);
		var attributesDiv = $(elemDiv).find('.student-attributes');
		
		var state = this.collapsed ? this.collapsedClass : this.expandedClass;
		$(elemDiv).addClass(state);
		
		if (!me.collapsed) {
		    me.appendStudentAttributes(attributesDiv, student, me.selectedAttributes);
		} else {
		    $(elemDiv).find('.student-attributes').hide();
		}

		var closeBtn = $(elemDiv).find('.del-button');
		$(closeBtn).click(function(event){
			me.removeStudent(student.id);
		});

		var fullProfileLink = $(elemDiv).find(this.fullProfileLinkClass);
		$(fullProfileLink).click(function (event) {
		    window.open("https://dashboard.sandbox.slcedu.org/s/l/student/" + student.id);                         
		});

		return elemDiv;		
	}
	
	/**
	 * TODO can optimize code to run faster 
	 * Add / Remove student attributes
	 */
	this.toggleStudentAttributeVisibility = function(selectedAttributes) {
		
		var me = this;
		$("#gc" + me.groupData.id + " .dropped-elem").each(function(index, item){			
			
		    var fullStudentData = student_grouping.studentsListComponent.students;
			var studentId = $(item).attr('data-studentId');
			var student = _.find(fullStudentData, function (s) {
				return s.studentData.id === studentId;
			});
			var studentData = student.studentData;
			
			var attributesDiv = $(item).find('.student-attributes');
			$(attributesDiv).empty();			
			me.appendStudentAttributes(attributesDiv, studentData, me.selectedAttributes);	
		});		
	}
	
    /** 
     * TODO refactor to make lookups generic
     * Show the given attributes on the students in this group
     */
	this.appendStudentAttributes = function(attributesDiv, studentData, attributes){
	    _.each(attributes, function (attribute) {
	        var name = attribute.attributeName;
	        var value = studentData[attribute.attributeId];
	        if (value === null || value === undefined || value === '' || value.length === 0) {
	            value = '[no data]';
	        }

            // perform lookup for sections
	        if (attribute.attributeId === 'sections') {
	            var sectionNames = [];
	            var studentSections = value;
	            var sections = student_grouping.sections;
	            _.each(studentSections, function (studentSection) {
	                // find the corresponding section using the id
	                var matchingSection = _.find(sections, function (section) {
	                    return section.id === studentSection;
	                });
	                if (matchingSection !== undefined) {
	                    sectionNames.push(matchingSection.courseTitle);
	                }
	            });
	            value = sectionNames;
	        }

			$(attributesDiv).append("<div><strong>" + name + "</strong> " + value + "</div>");
		});
	}

	/**
	 * Fill html template with group data
	 */
	this.generateTemplate = function(color){
		var groupData = this.groupData;
		var template = $(this.groupContainerTemplate);
		$(template).attr('id', 'gc' + groupData.id);

		var groupContainer = $(template).find(this.groupContainerClass);
		$(groupContainer).css('background-color', color.background);
		this.color = color;

		var groupNameLbl = $(groupContainer).find(this.groupNameLblClass);
		$(groupNameLbl).html(groupData.cohortIdentifier);
		
		var groupNameDiv = $(groupContainer).find(this.groupNameClass);
		$(groupNameDiv).css('background-color', color.title);
		
		var groupDiv = $(groupContainer).find(this.groupClass);
		$(groupDiv).attr('id', groupData.id);
		
		return template;
	}	

	/**
	 * Popup the menu for selecting the student data attributes to display
	 */
	this.showStudentDataPopup = function(){
		
		// hide the other popovers
		 $(this.groupInfoPopoverElem).hide();
		$(this.groupAttachmentPopoverElem).hide();		
		
		var groupContainerId = "gc" + this.groupData.id;
		var groupContainer = $("#" + groupContainerId);
		
		var popover = $(this.groupContainerId).find(this.studentPopoverElem);
		var popoverGroupContainerId = $(popover).attr('data-groupContainerId');
		
		// check if popover is already open 
		var notOpen = $(popover).css('display') === 'none';
		if (notOpen || groupContainerId !== popoverGroupContainerId) {	
			
			// place the popover relative to the group container
			var position_size = this.getPositionAndSize(); 
			
			$(popover).attr('data-groupContainerId', groupContainerId);
			$(popover).css('height', position_size.height);
			$(popover).css('display','');	
			
		    // attach event handler to hide this if user clicks outside of it
			this.handleOutsideClick(this.addDataBtnClass, this.studentPopoverElem);

		} else {
			// close it
			$(popover).css('display','none');
		}
		
		var me = this;
		var attributeCheckBoxes = this.studentPopoverElem + " .cbox-student-attribute";
		$(this.groupContainerId).find(attributeCheckBoxes).attr('checked', false);
		_.each(this.selectedAttributes, function(attribute){
		    $(me.groupContainerId).find(attributeCheckBoxes + "[value='" + attribute.attributeId + "']").attr('checked', true);
		});
		
		$(this.groupContainerId).find(attributeCheckBoxes).unbind('click');
		$(this.groupContainerId).find(attributeCheckBoxes).click(function (event) {
			var cbox = event.currentTarget;
			me.toggleSelectedAttributes();
			me.toggleStudentAttributeVisibility(me.selectedAttributes);
		})
	}
	
	/**
	 *  Repopulate the list of selected attributes with the user-selected attributes
	 */
	this.toggleSelectedAttributes = function(){
		var me = this;
		this.selectedAttributes = [];
		$(this.groupContainerId).find(this.studentPopoverElem + " .cbox-student-attribute").each(function(index, elem){
			var selected = $(elem).is(":checked");
			if (selected){
			    var val = $(elem).val();
			    var displayName = $(elem).attr('data-displayName');
			    me.selectedAttributes.push({ attributeId: val, attributeName: displayName });
			}
		});
		
		this.markDirty();
	}
	
	/**
	 * TODO optimize hidding code
	 * Popup the menu to show the group description 
	 */
	this.showMoreInfoPopup = function(){
		
		// hide the other popovers
		$(this.studentPopoverElem).hide();
		$(this.groupAttachmentPopoverElem).hide();		
		
		var groupContainerId = "gc" + this.groupData.id;
		var groupContainer = $("#" + groupContainerId);
		
		var popover = $(this.groupContainerId).find(this.groupInfoPopoverElem);
		var popoverGroupContainerId = $(popover).attr('data-groupContainerId');
		
		// check if popover is already open 
		var notOpen = $(popover).css('display') === 'none';
		if (notOpen || groupContainerId !== popoverGroupContainerId) {		
			
		    var description = this.groupData.cohortDescription;
		    if (description !== null && description !== '') {
		        $(this.groupDescriptionTxtElem).html(description);
		    } else {
		        $(this.groupDescriptionTxtElem).html('&nbsp;');
		    }						
			
			// place the popover relative to the group container
			var position_size = this.getPositionAndSize(); 
			
			$(popover).attr('data-groupContainerId', groupContainerId);
			$(popover).css('height', position_size.height);
			$(popover).css('display', '');
			
			// if user clicks on text, make it editable					
			$(this.groupContainerId).find(this.groupDescriptionTxtElem).click(function(event){
				me.makeGroupDescriptionEditable();
			});

		    // attach event handler to hide this if user clicks outside of it
			this.handleOutsideClick(this.groupInfoBtnClass, this.groupInfoPopoverElem);
				
		} else {
			// close it
			$(popover).css('display','none');
		}	
	}
	
	/**
	 * TODO match width of panel to width of container
     * Popup the attachment panel
	 */
	this.showAttachmentPopover = function(){
		
		if (me.attachedFile === null){
			var groupContainerId = "gc" + this.groupData.id;
			var groupContainer = $("#" + groupContainerId);
			
			var popover = $(this.groupContainerId).find(this.groupAttachmentPopoverElem);
			var popoverGroupContainerId = $(popover).attr('data-groupContainerId');
			
			// check if popover is already open
			var notOpen = $(popover).css('display') === 'none';
			if (notOpen || groupContainerId !== popoverGroupContainerId) {
				
				// place the popover relative to the group container
				var position = $(groupContainer).offset();
				var height = $(groupContainer).height();
				
				$(popover).attr('data-groupContainerId', groupContainerId);
				$(popover).css('display', '');
							
				$(this.groupContainerId).find(this.groupAttachmentPopoverDoneBtnElem).unbind('click');
				$(this.groupContainerId).find(this.groupAttachmentPopoverDoneBtnElem).click(this.attachFile);
				
				$(this.groupContainerId).find(this.groupAttachmentPopoverFileInput).val('');
				$(this.groupContainerId).find(this.groupAttachmentPopoverFileTxt).val('');
				$(this.groupContainerId).find(this.groupAttachmentPopoverFileInput).unbind('change');
				$(this.groupContainerId).find(this.groupAttachmentPopoverFileInput).change(function () {
				    $(document).unbind('mouseup');
				    var fileName = $(me.groupContainerId).find(me.groupAttachmentPopoverFileInput).val()
				    $(me.groupContainerId).find(me.groupAttachmentPopoverFileTxt)
                        .val(fileName);
				});

			    // attach event handler to hide this if user clicks outside of it
				this.handleOutsideClick(this.groupAttachmentImgClass, this.groupAttachmentPopoverElem);
			} else {
				// close it
				$(popover).css('display', 'none');
			}
		}		
	}
	
	/**
	 * Attach the user specified file to this   
	 */
	this.attachFile = function(event){
	    var files = $(me.groupContainerId).find(me.groupAttachmentPopoverFileInput).prop('files');
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
		        me.attachedFile = {
		        	name: file.name,
		        	type: type,
		        	content: content
		        }

		        // show the div with the attachment
		        me.showFileAttachment();
		      };
		    })(file);
		
		    // Read in the image file as a data URL.
		    reader.readAsDataURL(file);
			
			// hide the popover			
			$(me.groupContainerId).find(me.groupAttachmentPopoverElem).hide();	
			
			
			me.markDirty();			
		} else {
			me.attachedFile = null;
		}		
	}
	
	/**
	 * Remove the attachment from the group 
	 */
	this.deleteAttachment = function(event){
		me.attachedFile = null;
		$(me.groupContainerId).find(me.groupAttachmentNameClass).html('');
		$(me.groupContainerId).find(me.groupAttachmentDivClass).hide();
		
		this.markDirty();
	}	
	
    /**
     * TODO implement better solution for cutting off long file names
	 * Show the attached file
	 */
	this.showFileAttachment = function(){
	    var file = this.attachedFile;
	    if (file !== null && file !== undefined) {
	        $(me.groupContainerId).find(me.groupAttachmentNameClass).attr('href', file.type + "," + file.content);
	        $(me.groupContainerId).find(me.groupAttachmentNameClass).attr('download', file.name);

	        var fileName = file.name;
            // cut off filename if its too long
	        if (fileName.length > 28) {
	            fileName = file.name.substring(0, 28) + "...";
	        }
	        $(me.groupContainerId).find(me.groupAttachmentNameClass).html(fileName);
	        $(me.groupContainerId).find(me.groupAttachmentDivClass).show();
	    }
	}
	
	/**
	 * Toggle student expanded/collapsed state 
	 */
	this.toggleStudentState = function() {		
		
		var classToRemove = this.collapsed ? this.collapsedClass : this.expandedClass;
		var classToAdd = this.collapsed ? this.expandedClass : this.collapsedClass;
		
		var droppedElem = $(this.groupContainerId).find(this.droppedElemClass);
		$(droppedElem).removeClass(classToRemove);
		$(droppedElem).addClass(classToAdd);		
		
		this.collapsed = !this.collapsed;
		
		// show/hide student attributes
		this.collapsed ? $(droppedElem).find(this.studentAttributesClass).hide() 
			: $(droppedElem).find(this.studentAttributesClass).show();		
		
		var btnTxt = this.collapsed ? 'expand' : 'collapse';
		$(this.groupContainerId).find(this.expColGroupBtnClass).html(btnTxt);		
	}
	
	/**
	 * Close the group 
	 */
	this.closeGroup = function(){
		
		if (this.dirty){				
		    $(this.groupUnsavedChangesGroupName).html(this.groupData.cohortIdentifier);
		    
			$(this.groupUnsavedChangesConfirmBtnElem).unbind('click');
			$(this.groupUnsavedChangesConfirmBtnElem).click(function(){
				me.dirty = false;
				me.closeGroup();
			});
			
			$(this.groupUnsavedChangesModalElem).modal('show');
		} else {		
			$(this.groupContainerId).remove();		
			this.pubSub.publish('group-removed', this.groupData.id);	
		}
		
	}
	
	/**
	 * Make the group name label editable, turns it into a textbox
	 */
	this.makeGroupNameEditable = function () {
		var groupName = $(this.groupContainerId).find(this.groupNameLblClass).html();
		$(this.groupContainerId).find(this.groupNameLblClass).hide();		
		$(this.groupContainerId).find(this.groupNameTxtClass)
			.val(groupName)
			.css('display', '')
			.focus();
											
		$(this.groupContainerId).find(this.groupNameTxtClass).unbind('blur');
		$(this.groupContainerId).find(this.groupNameTxtClass).blur(function(event){
			me.saveGroupName();
		});
	}
	
	/**
	 * Save the new group name 
	 */
	this.saveGroupName = function(){
	    var newGroupName = $(this.groupContainerId).find(this.groupNameTxtClass).val();

	    // if no input, then set default name
	    if (!/\S/.test(newGroupName)) {
	        newGroupName = 'New Group';
	    }
		this.groupData.cohortIdentifier = newGroupName;
		$(this.groupContainerId).find(this.groupNameLblClass).html(newGroupName);
		$(this.groupContainerId).find(this.groupNameLblClass).show();
		$(this.groupContainerId).find(this.groupNameTxtClass).hide();
		
		this.markDirty();
	}
	
	/**
	 * Make the group description text editable, turns it into a textarea
	 */
	this.makeGroupDescriptionEditable = function(){
		var groupDescription = this.groupData.cohortDescription;
		$(this.groupContainerId).find(this.groupDescriptionTxtElem).hide();
		
		var height = $(this.groupContainerId).find(this.groupInfoPopoverElem).css('height').replace('px', '');
		$(this.groupContainerId).find(this.groupDescriptionTxtAreaElem).css('height', parseInt(height) - 40);
	    $(this.groupContainerId).find(this.groupDescriptionTxtAreaElem)
			.val(groupDescription)
			.show()
			.focus();		
			
	    $(this.groupContainerId).find(this.groupDescriptionTxtAreaElem).unbind('blur');
	    $(this.groupContainerId).find(this.groupDescriptionTxtAreaElem).blur(function (event) {
			me.saveGroupDescription();
		});	
	}
	
	/**
	 *  Save the new group description
	 */
	this.saveGroupDescription = function(){

	    var newGroupDescription = $(this.groupContainerId).find(this.groupDescriptionTxtAreaElem).val();
	    this.groupData.cohortDescription = newGroupDescription;
	    $(this.groupContainerId).find(this.groupDescriptionTxtElem).html(newGroupDescription);
	    $(this.groupContainerId).find(this.groupDescriptionTxtAreaElem).hide();
	    $(this.groupContainerId).find(this.groupDescriptionTxtElem).show();

	    this.markDirty();	    
	}
	
	
	/**
	 * Returns the position and size of this group's container element 
	 */
	this.getPositionAndSize = function(){
		var position = $(this.groupContainerId).offset();
		var width = $(this.groupContainerId).find(this.groupContainerClass).width(); 
		var height = $(this.groupContainerId).find(this.groupContainerClass).height();
				
		var position_size = {
			left: position.left,
			top: position.top,
			width: width,
			height: height
		}			
		return position_size;
	}
	
	/**
	 * Mark this group as dirty so that changes are saved back to server 
	 */
	this.markDirty = function(){
		this.dirty = true;
	}

    /**
     * Handle outside click event to hide popover
     */
	this.handleOutsideClick = function (triggetBtn, container){
	    $(document).unbind('click');
	    $(document).click(function (e) {
	        if ((!$(container).is(e.target) && $(container).has(e.target).length === 0)
                    && (!$(triggetBtn).is(e.target) && $(triggetBtn).has(e.target).length === 0)) {
	            $(container).hide();
	            $(document).unbind('click');
	        }
	    });
	}
    
    /**
     * Add new group or update a group
     */
	this.saveGroupChanges = function () {

	    var cohortActionObject = this.prepareGroupForSaving();

	    // negative ids represent new groups
	    var method = 'CreateGroup';
	    var successHandler = null;
	    var errorHandler = null;
	    if (parseInt(this.groupData.id) < 0) {
	        cohortActionObject.cohort.id = null;
	        successHandler = me.createGroupSuccessHandler;
	        errorHandler = me.createGroupErrorHandler;
	    } else {
	        method = 'UpdateGroup';
	        successHandler = me.updateGroupSuccessHandler;
	        errorHandler = me.updateGroupErrorHandler;
	    }

	    $.ajax({
	        type: 'POST',
	        url: method,
	        contentType: 'application/json',
	        data: JSON.stringify(cohortActionObject),
	        success: function (result) {
	            if (result.completedSuccessfully) {
	                successHandler(result);
	            } else if (!result.partialCreateSuccess || !result.partialDeleteSuccess || !result.customActionResult.isSuccess){
	                errorHandler(result);
	            }
	        },
            error: function(result) {
                // should implement exception handling
            }
	    });
	    me.toggleGroupContainerProcessingState(true);
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

	    var studentsToDelete = _.filter(this.originalStudents, function (origStudentId) {
	        var matchingStudent = _.find(me.students, function (student) {
	            return origStudentId === student;
	        });
	        return matchingStudent === undefined;
	    });

        // if negative, then it is a new group so it doesn't have an id
	    var id = parseInt(this.groupData.id) > 0 ?
             this.groupData.id : null;
        
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
     * Delete this group permanently
     */
	this.deleteGroup = function () {

	    var groupId = me.groupData.id;
	    var confirmation = confirm('Are you sure you want to delete the group: ' + me.groupData.cohortIdentifier);
	    if (confirmation) {
	        // make sure we are not deleting newly created, unsaved groups
	        if (groupId < 0) {
	            // just delete the group locally since it was not saved to the server
	            me.deleteGroupSuccessHandler(null);
	        } else {
	            $.ajax({
	                type: 'POST',
	                url: 'DeleteGroup?id=' + groupId,
	                contentType: 'application/json',
	                success: function (result) {
	                    me.deleteGroupSuccessHandler(result);
	                },
	                error: function (result) {
                        // TODO if there is id then set it
	                    me.deleteGroupErrorHandler(result);
	                }
	            });
	        }

	        me.toggleGroupContainerProcessingState(true);
	    }
	}

    /**
     * Handle successful saving of group
     */
	this.createGroupSuccessHandler = function(result) {
	    me.groupData.id = result.objectId;

	    // Let user know the created was successful
	    utils.uiUtils.showTooltip(
            $(me.groupContainerId).find(me.groupNameLblClass),
            'Group has been successfully created.',
            'top',
            'manual',
            3000);

	    me.toggleGroupContainerProcessingState(false);
	    me.dirty = false;

	    // put the current list of students into the original list
	    _.each(me.students, function(student){
	        me.originalStudents.push(student);
	    });

	    // request to be added to list of existing groups
	    me.pubSub.publish('add-to-existing-groups', me.groupData);
	}

    /**
     * Handle error with saving a group
     */
	this.createGroupErrorHandler = function (result) {
	    me.toggleGroupContainerProcessingState(false);

	    var msg = "Group could not be created. Please try again later or contact your system administrator.";

	    if (result.completedSuccessfully && (result.failToCreateIds.length > 0)){
	        msg = "Group was created successfully. However some students could not be assigned to the group.";
	    }

	    // Let user know the create was not successful
	    utils.uiUtils.showTooltip(
            $(me.groupContainerId).find(me.groupNameLblClass),
            msg,
            'top',
            'manual',
            3000);
	}

    /**
     *
     */
	this.updateGroupSuccessHandler = function (result) {
	    me.toggleGroupContainerProcessingState(false);

        // Let user know the save was successful
	    utils.uiUtils.showTooltip(
            $(me.groupContainerId).find(me.groupNameLblClass),
            'Group has been successfully updated.',
            'top',
            'manual',
            3000);

	    me.dirty = false;
	}

    /**
     *
     */
	this.updateGroupErrorHandler = function (result) {
	    me.toggleGroupContainerProcessingState(false);
	    // Let user know the update was not successful
	    utils.uiUtils.showTooltip(
            $(me.groupContainerId).find(me.groupNameLblClass),
            'Group could not be updated. Please try again later or contact your system administrator.',
            'top',
            'manual',
            3000);
	}

    /**
     *
     */
	this.deleteGroupSuccessHandler = function (result) {
	    me.pubSub.publish('group-deleted', me.groupData.id);

	    // Let user know the delete was successful
	    utils.uiUtils.showTooltip(
            $(me.groupContainerId).find(me.groupNameLblClass),
            'Group has been successfully deleted.',
            'top',
            'manual',
            2000);
	    setTimeout(function () {
	        me.toggleGroupContainerProcessingState(false);
	        $(me.groupContainerId).remove();
	    }, 2000);
	}

    /**
     *
     */
	this.deleteGroupErrorHandler = function (result) {
	    me.toggleGroupContainerProcessingState(false);
	    // Let user know the delete was not successful
	    utils.uiUtils.showTooltip(
            $(me.groupContainerId).find(me.groupNameLblClass),
            'Group could not be deleted. Please try again later or contact your system administrator.',
            'top',
            'manual',
            3000);
	}

    /**
     * If doing an ajax request, fade out the background and display spinner
     */
	this.toggleGroupContainerProcessingState = function(processing){
	    if (processing){
	        $(me.groupContainerId).css('opacity', 0.5);
	        $(me.groupContainerId).spin();
	    } else {
	        $(me.groupContainerId).css('opacity', 1);
	        $(me.groupContainerId).spin(false);
	    }
	    this.processing = processing;
	}

    /**
     * Update the label that shows the number of students in group
     */
	this.updateNumStudentsBadge = function () {
	    var numStudents = me.students.length;
	    $(me.groupContainerId).find(me.groupNumStudentsBadgeClass).css('background-color', me.color.title);
	    $(me.groupContainerId).find(me.groupNumStudentsBadgeClass).html(numStudents);
	}

    /**
     * Remove all the students from this group
     */
	this.removeAllStudents = function () {
	    var students = this.students;
	    _.each(students, function (student) {
	        me.removeStudent(student);
	    });
	}

    /**
     * Update this group's id
     */
	this.updateId = function (id) {
	    var origGroupContainerId = $("#gc" + me.groupData.id);
	    me.groupData.id = id;
	    me.groupContainerId = "#gc" + id;
	    $(origGroupContainerId).find(me.groupClass).attr('id', id);
	    $(origGroupContainerId).attr('id', "gc" + id);
	}
}
