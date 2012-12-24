var student_grouping = student_grouping || {};

student_grouping.groupListWidget = function () {
    var me = this;
    this.pubSub = PubSub;

    // keep track of ALL the group models, regardless of whether they are in the current workspace or not
    this.groupListModel = null;

    // keep track of the current group we are dragging to
    this.currGrp = null;
    this.groupWidgets = [];
    this.dataElements = [];
    this.lastNewGroupIndex = -1;

    this.groupClass = '.group';
    this.groupsAreaClass = '.groups-area';

    this.saveAllGroupsModalElem = "#save-all-summary";
    this.saveAllGroupsContentElem = "#save-all-summary-content";

    this.colorList = [];
    this.currentColorIndex = 0;

    /**************************
     * METHODS
     **************************/
    /**
     * Initialize this widget
     * @param groupModels - client-side groups
     * @param colors
     * @param dataElements
     */
    this.init = function (groupModels, colors, dataElements) {

        // store pre-defined data elements from backend
        _.each(dataElements, function (dataElement) {
            me.dataElements.push(dataElement);
        });

        // store pre-defined colors from backend
        _.each(colors, function (color) {
            me.colorList.push(color);
        });

        me.groupListModel = new student_grouping.groupListModel();
        me.groupListModel.addGroups(groupModels);

        // // create number of groups specified from previous screen
        var urlParams = utils.uiUtils.getUrlParams();
        var numNewGroups = urlParams.create;
        if (numNewGroups !== undefined && numNewGroups !== null) {
            for (var i = 0; i < numNewGroups; i++) {
                me.addNewGroup();
            }
        }

        // add the groups passed from the prev screen
        var selGroups = urlParams.selGroups;
        if (selGroups !== undefined && selGroups !== null) {
            var selGroupsArr = selGroups.split(',');
            _.each(selGroupsArr, function (selGroupId) {                
                var selGroup = me.findGroupModel(selGroupId);
                if (selGroup !== undefined) {
                    me.addGroup(selGroup);
                }
            });
        }

        this.pubSub.subscribe('add-new-group', function () {
            me.addNewGroup();
        });

        this.pubSub.subscribe('add-group', function (group) {
            var newGroup = me.addGroup(group);
        });

        this.pubSub.subscribe('add-to-existing-groups', function (groupModel) {
            me.groupListModel.addGroup(groupModel);
        });

        this.pubSub.subscribe('group-removed', function (groupId) {
            me.removeGroup(groupId);
        });

        this.pubSub.subscribe('group-deleted', function (groupId) {
            me.removeGroup(groupId);
        });

        this.pubSub.subscribe('assign-random', function (students, numInGroup) {
            me.assignRandomGroups(students, numInGroup);
        });

        this.pubSub.subscribe('print-all-groups', function () {
            me.printAllGroups();
        });

        this.pubSub.subscribe('save-all-groups', function () {
            me.saveAllGroups();
        });

        this.pubSub.subscribe('has-dirty-groups-huh', function (callback) {
            var hasDirtyGroups = me.hasDirtyGroups();
            callback(hasDirtyGroups);
        });
    }

    /** 
     * Add the group with the given id to the current workspace
     * @param groupId
     */
    this.addGroupById = function (groupId) {
        var groupModel = me.groupListModel.getGroupById(groupId);
        if (groupModel !== undefined && groupModel !== null) {
            me.addGroup(groupModel);
        }
    }

    /**
     * Add given group to the list 
     * @param groupModel - client-side group
     * @return the newly added group
     */
    this.addGroup = function (groupModel) {

        // check if group has already been added
        var groupInWorkspace = me.groupInWorkspace();

        if (!groupInWorkspace) {
            groupModel.init();
            var groupWidget = new student_grouping.groupWidget(groupModel);
            var color = me.colorList[me.currentColorIndex++];
            $(me.groupsAreaClass).append(groupWidget.generateTemplate(color));
            groupWidget.init(me.dataElements);

            if (me.currentColorIndex >= me.colorList.length) {
                me.currentColorIndex = 0;
            }

            // make group droppable
            $(groupWidget.groupContainerId).find('.group-wrap').droppable({
                drop: function (event) {
                    var groupDiv = $(groupWidget.groupContainerId).find('.group');
                    me.dropFunc(groupDiv);
                }
            });

            me.groupWidgets[groupModel.getId()] = groupWidget;

            return groupWidget;
        } else {
            //TODO tell user group is already there
        }

        return null;
    }

    /**
     * Creates and adds a new empty, unsaved group
     */
    this.addNewGroup = function () {
        var newGroupModel = me.createNewGroupModel();
        me.addGroup(newGroupModel);
    }

    /**
     * Remove the given group from the list 
     */
    this.removeGroup = function (groupId) {
        delete me.groupWidgets[groupId];
    }

    /**
	 *Handle the drop action on a group 
	 */
    this.dropFunc = function (groupContainer) {
        // we use currGrp to make sure that the students get dropped once
        // need to do this because the dropFunc is called for each dragged student
        if (me.currGrp === null) {
            var groupId = $(groupContainer).attr('id');
            me.currGrp = groupId;

            // find the group widget
            var groupWidget = me.groupWidgets[groupId];

            // loop through each student being dragged
            $(".ui-draggable-dragging").each(function (index, elem) {

                var studentId = $(elem).attr('data-studentId');

                // find student obj TODO - refactor dependency
                var studentModel = student_grouping.studentListWidgetComponent.getStudentById(studentId);

                // check if are moving between groups
                if ($(elem).hasClass('dropped-elem')) {

                    // find the group me belongs to 
                    var originalGroupId = $(elem).parents('.group').attr('id');

                    // check if target group is indeed a different group
                    if (originalGroupId !== groupId) {
                        // find the group object
                        var originalGroupWidget = me.groupWidgets[originalGroupId];                        

                        // check that target group doesnt aleady have the student					
                        var studentId = $(elem).attr('data-studentId');
                        var groupHasStudent = groupWidget.hasStudent(studentId);

                        if (!groupHasStudent) {
                            originalGroupWidget.removeStudent(studentId);
                            originalGroupWidget.markDirty();
                        }
                    }
                }

                // assign student to group
                groupWidget.assignStudentToGroup(studentModel);
                groupWidget.markDirty();
            });
        }
    }

    /**
	 * Randomly assign the given students into groups of the specified number  
	 */
    this.assignRandomGroups = function (studentModels, numInGroup) {
        studentModels = utils.arrayUtils.shuffle(studentModels);
        var groupWidgets = utils.arrayUtils.shuffle(me.groupWidgets);
        var numGroups = groupWidgets.length;

        // remove students from all groups
        for (var groupId in groupWidgets) {
            var groupWidget = groupWidgets[groupId];
            groupWidget.removeAllStudents();
        }

        // keep track of the next group that is not FULL
        var nextAvailableGroup = null;
        _.each(studentModels, function (studentModel) {
            var studentId = studentModel.getId();

            // keep track of whether student has been assigned to the existing groups
            var addedToGroup = false;
            for (var groupId in groupWidgets) {
                var groupWidget = groupWidgets[groupId];
                var numStudents = groupWidget.getNumberOfStudents();
                if (numStudents < numInGroup) {
                    groupWidget.assignStudentToGroup(studentModel);

                    addedToGroup = true;
                    break;
                }
            }           

            // should create a new group if student was not added to any existing group
            if (!addedToGroup) {

                var groupModel = me.createNewGroupModel();
                var newGroupWidget = me.addGroup(groupModel);
                newGroupWidget.assignStudentToGroup(studentModel);
                newGroupWidget.markDirty();
                numGroups++;
            }
        });
    }

    /**
	 * Returns true if the given group exists in this component's list of groups 
	 */
    this.containsGroup = function (groupId) {
        var existingGroup = me.groupWidgets[groupId];
        return existingGroup !== undefined;
    }

    /**
     * TODO server side will handle all this with one call
     * Saving changes to all groups
     */
    this.saveAllGroups = function () {
        var groupWidgets = me.groupWidgets;

        var groupsToSave = [];
        var originalGroupsToSave = []; // keep track of the actual group objects so we can later update with created ID
        for (var groupId in groupWidgets) {
            var groupWidget = groupWidgets[groupId];
            // find dirty and new groups
            var groupModel = groupWidget.groupModel;
            if (groupWidget.dirty || groupModel.isNewGroup()) {
                var cohortActionObject = groupModel.prepareGroupForSaving();
                groupsToSave.push(cohortActionObject);
                originalGroupsToSave.push(groupWidget);
            }
        }

        // make sure there are dirty groups to save
        if (groupsToSave.length === 0) {
            alert("There are no groups to save. Please add some existing groups to modify or create new ones before saving.");
            me.saveAllComplete();
            return;
        }

        // disable the groups area screen TODO externalize this code
        $(me.groupsAreaClass).spin();
        $(me.groupsAreaClass).css('opacity', 0.2);

        // call server to add all new groups
        $.ajax({
            type: 'POST',
            url: 'SaveAll',
            contentType: 'application/json',
            data: JSON.stringify(groupsToSave),
            success: function (results) {
                me.saveAllGroupsSuccessHandler(results, originalGroupsToSave);
            },
            error: me.saveAllGroupsErrorHandler
        });
    }

    /**
     * TODO refactor assign ids to new groups
     */
    this.saveAllGroupsSuccessHandler = function (results, groupsToSave) {
        var numSuccessfulSaves = 0;
        var numResults = results.length;
        var successDiv = $("<ul>");
        var failDiv = $("<ul>");
        for (var i = 0; i < numResults; i++) {
            var result = results[i];
            var groupWidget = groupsToSave[i];

            // assign id to newly created groups
            if (result.objectActionResult.objectId !== null && result.objectActionResult.objectId !== undefined) {
                groupWidget.updateId(result.objectActionResult.objectId);
            }

            // sync group original data with new changes
            groupWidget.groupModel.saveResultHandler(result);

            if (result.completedSuccessfully) {
                numSuccessfulSaves++;
                groupWidget.dirty = false;

                $(successDiv).append("<li>Group Name - " + result.objectActionResult.objectName + "</li>");
            } else {
                var groupListItem = $("<li>");
                // check if group was successfully saved
                var groupObjResult = result.objectActionResult;
                if (groupObjResult.isSuccess) {
                    $(groupListItem).append("<div>" + groupObjResult.objectName + " was saved successfully </div>");
                } else {
                    $(groupListItem).append("<div>" + groupObjResult.objectName + " was not saved successfully </div>");
                }

                var failToCreate = result.failToCreateAssociations;
                if (failToCreate !== null && failToCreate.length > 0) {
                    $(groupListItem).append("<div>Failed to add these students: </div>");
                    var failToCreateList = $("<div>");
                    _.each(failToCreate, function (fail) {
                        $(failToCreateList).append(fail.objectName + ", ");
                    });
                    $(groupListItem).append(failToCreateList);
                }

                var failToDelete = result.failToDeleteAssociations;
                if (failToDelete !== null && failToDelete.length > 0) {
                    $(groupListItem).append("<div>Failed to remove these students: </div>");
                    var failToDeleteList = $("<div>");
                    _.each(failToCreate, function (fail) {
                        $(failToDeleteList).append(fail.objectName + ", ");
                    });
                    $(groupListItem).append(failToDeleteList);
                }

                $(failDiv).append(groupListItem);
            }
        }

        var successDiv = $("<div class='well label-success save-all-msg'><div>Number of successful saves: " + numSuccessfulSaves + "</div>").append(successDiv);
        var failDiv = $("<div class='well label-important save-all-msg'><div>Number of unsuccessful saves: " + (numResults - numSuccessfulSaves) + "</div>").append(failDiv);

        $(me.saveAllGroupsContentElem).empty();
        $(me.saveAllGroupsContentElem).append(successDiv).append(failDiv);
        $(me.saveAllGroupsModalElem).modal('show');

        me.saveAllComplete();
    }

    /**
     * TODO need better way to display errors
     */
    this.saveAllGroupsErrorHandler = function (errorMsg) {
        $(me.saveAllGroupsModalElem).modal('show');
        $(me.saveAllGroupsContentElem).html("<div>There was an error saving your changes.</div> <div>Error message: " + errorMsg.responseText + "</div>");

        me.saveAllComplete();
    }

    this.saveAllComplete = function () {

        // notify others that save all has completed
        me.pubSub.publish('save-all-completed');

        // re-enable the groups area screen
        $(me.groupsAreaClass).spin(false);
        $(me.groupsAreaClass).css('opacity', 1);
    }

    /**
     * Creates a new empty group object
     */
    this.createNewGroupModel = function () {        
        var groupData = {
            cohort: {
                id: me.lastNewGroupIndex--,
                cohortIdentifier: 'New Group',
                cohortDescription: ''
            },
            students: [],
            custom: {
                lessonPlan: null,
                dataElements: null
            }
        };
        var groupModel = new student_grouping.groupModel(groupData);
        return groupModel;
    }

    /**
     * Returns true if there are dirty groups
     */
    this.hasDirtyGroups = function () {
        var dirty = false;
        var groupWidgets = me.groupWidgets;
        for (var groupId in groupWidgets) {
            var groupWidget = groupWidgets[groupId];
            if (groupWidget.dirty) {
                dirty = true;
                break;
            }
        }

        return dirty;
    }

    /**
     * Print all the groups in the workspace
     */
    this.printAllGroups = function () {
        var div = $("<div>");

        var groupWidgets = me.groupWidgets;
        for (var groupId in groupWidgets) {
            var groupWidget = groupWidgets[groupId];
            var groupDiv = groupWidget.generatePrintableHtml();
            $(div).append(groupDiv);
        }

        utils.printUtils.print($(div).html());
    }

    /**
     * Returns true if the group has already been added to the workspace
     */
    this.groupInWorkspace = function (groupId) {
        return me.groupWidgets[groupId] !== undefined;
    }

    /**
     * Returns the group model with the given id
     */
    this.findGroupModel = function (groupId) {
        return me.groupListModel.getGroupById(groupId);
    }

    /**
     * Returns true if the given group name already exists
     */
    this.groupNameExists = function (groupName) {
        var group = me.groupListModel.getGroupByName(groupName);
        return group !== undefined;
    }
}