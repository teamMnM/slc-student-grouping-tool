﻿var student_grouping = student_grouping || {};

student_grouping.groupListItemWidget = function (groupModel) {
    var me = this;
    this.pubSub = PubSub;

    this.groupModel = groupModel;
    this.dirty = false;

    this.containerId = '';
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
     * SETUP METHODS
     **************************/
    this.init = function () {
        me.groupContainerId = "#" + me.groupModel.getId();
        
        me.setupEventHandlers();
        me.setupSubscriptions();
        me.groupModel.init();
        me.showFileAttachment();
    }

    /**
     * Sets up the event handlers for user interaction with the widget
     */
    this.setupEventHandlers = function () {
        $(me.groupContainerId).click(function (event) {
            var processing = group_selection.groupDetailsComponent.processing;
            if (!processing) {
                me.groupSelected();
            }
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
    }

    /**
     * Sets up listeners for pubsub events
     */
    this.setupSubscriptions = function () {
        me.pubSub.subscribe('remove-student', function (studentId) {
            me.removeStudent(studentId);
        });
    }

    /**************************
     * METHODS
     **************************/
    /**
	 * Return the HTML content for this object 
	 */
    this.generateTemplate = function () {
        var groupData = me.groupModel.groupData;
        var template = $(me.groupTemplate);

        $(template).attr('id', groupData.id);
        $(template).find(me.groupTitleClass).html(groupData.cohortIdentifier);

        var lastModifiedDateStr = me.groupModel.getLastModTimeString();
        $(template).find(me.groupModifiedTimestampClass).html('Last modified: ' + lastModifiedDateStr);
        $(template).find(me.groupDescriptionClass).html(groupData.cohortDescription);

        return template;
    }


    /**
     * TODO implement better solution for cutting off long file names
	 * Show the attached file
	 */
    this.showFileAttachment = function () {
        var file = me.groupModel.attachedFile;
        if (file !== null && file !== undefined) {
            $(me.groupContainerId).find(me.groupAttachmentLinkClass).attr('href', file.type + "," + file.content);
            $(me.groupContainerId).find(me.groupAttachmentLinkClass).attr('download', file.name);

            $(me.groupContainerId).find(me.groupAttachmentLinkClass).show();
        } else {
            $(me.groupContainerId).find(me.groupAttachmentLinkClass).hide();
        }
    }

    /**
     * Delete this group 
     */
    this.deleteGroup = function () {
        var groupName = me.groupModel.groupData.cohortIdentifier;
        var confirmation = confirm('Are you sure you want to delete the group: ' + groupName);
        if (confirmation) {
            // make sure we are not deleting newly created, unsaved groups
            if (me.groupModel.isNewGroup()) {
                me.deleteGroupSuccessHandler(null);
            } else {
                me.groupModel.delete(me.deleteGroupSucessHandler, me.deleteGroupErrorHandler);                
            }
            me.toggleGroupContainerProcessingState(true);
        }
    }


    /**
     * Callback handler for successful delete
     */
    this.deleteGroupSuccessHandler = function () {
        me.pubSub.publish('group-deleted', me.groupModel.getId());

        // Let user know the delete was successful
        utils.uiUtils.showTooltip(
            $(me.groupContainerId).find(me.groupDeleteIconClass),
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
            $(me.groupContainerId).find(me.groupDeleteIconClass),
            'Group could not be deleted. Please try again later or contact your system administrator.',
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
            $(me.groupContainerId).css('opacity', 0.5);
            $(me.groupContainerId).spin();
        } else {
            $(me.groupContainerId).css('opacity', 1);
            $(me.groupContainerId).spin(false);
        }
        me.processing = processing;
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