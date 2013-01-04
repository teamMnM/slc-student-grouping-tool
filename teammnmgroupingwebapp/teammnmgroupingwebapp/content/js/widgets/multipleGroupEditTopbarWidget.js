﻿var student_grouping = student_grouping || {};

student_grouping.multipleGroupEditTopbarWidget = function () {
    var me = this;
    this.pubSub = PubSub;

    this.topbarControls = ".top-bar-controls";
    this.backBtnElem = '#back-btn';
    this.findGroupDropdownElem = '#find-group-dropdown';
    this.findGroupSelect2Elem = '#s2id_find-group-dropdown'; // this could change with plugin update - unstable
    this.addExistingGroupBtn = '#add-existing-group-btn';
    this.addNewGroupBtn = '#add-new-group-btn';
    this.printBtnElem = '#img-print-btn';
    this.saveAllBtnElem = '#img-save-btn';
    this.logoutBtnElem = '#logout-btn';

    this.savingAll = false;
    this.groupModels = [];

    /**************************
     * SETUP METHODS
     **************************/
    this.init = function (groupModels) {

        // set up the existing groups dropdown
        _.each(groupModels, function (groupModel) {
            me.groupModels[groupModel.getId()] = groupModel;
            var cohortData = groupModel.groupData;
            $(me.findGroupDropdownElem).
    			append("<option value='" + cohortData.id + "'>" + cohortData.cohortIdentifier + "</option>");
        });

        $(me.findGroupDropdownElem).select2();
        setTimeout(function () {
            $(".select2-container").not('.span11').width('100%');
        }, 500);

        me.setupEventHandlers();
        me.setupSubscriptions();
    }

    /**
     * Sets up the event handlers for user interaction with the widget
     */
    this.setupEventHandlers = function () {
        $(me.backBtnElem).click(me.navigateBack);
        $(me.addExistingGroupBtn).click(me.addExistingGroup);
        $(me.addNewGroupBtn).click(me.addNewGroup);
        $(me.printBtnElem).click(me.printAllGroups);
        $(me.saveAllBtnElem).click(me.saveAllGroups);
        $(me.logoutBtnElem).click(me.logout);
    }

    /**
    * Sets up listeners for pubsub events
    */ 
    this.setupSubscriptions = function () {
        // remove group from dropdown if deleted
        me.pubSub.subscribe('group-deleted', function (id) {
            me.removeGroup(id);
        });

        // TODO add description
        me.pubSub.subscribe('save-all-completed', function () {
            me.savingAll = false;
        });

        // add newly created (saved to server) group to dropdown list
        me.pubSub.subscribe('add-to-existing-groups', function (groupModel) {
            $(me.findGroupDropdownElem).
    			append("<option value='" + groupModel.getId() + "'>" + groupModel.groupData.cohortIdentifier + "</option>");
            me.groupModels[groupModel.getId()] = groupModel;
        });

        // update name of group if it changed
        me.pubSub.subscribe('group-changed', function (originalId, groupModel) {
            var optionElem = $(me.findGroupDropdownElem).find("option[value='" + originalId + "']");
            $(optionElem).attr('value', originalId);
            $(optionElem).html(groupModel.groupData.cohortIdentifier);
        });
    }

    /**************************
     * METHODS
     **************************/
    /**
     *  
     */
    this.addExistingGroup = function (event) {

        // check if a group has been selected
        var selGroupId = $(me.findGroupDropdownElem).val();
        var groupAdded = student_grouping.groupListWidgetComponent.containsGroup(selGroupId);

        if (selGroupId === '') {
            // create tooltip on the fly
            $(me.findGroupSelect2Elem).tooltip('destroy');
            $(me.findGroupSelect2Elem).tooltip({
                title: 'please select a group to add',
                placement: 'bottom',
                trigger: 'manual'
            });

            $(me.findGroupSelect2Elem).tooltip('show');
            setTimeout(function () {
                $(me.findGroupSelect2Elem).tooltip('hide');
            }, 3000);
        } else if (groupAdded) {
            // create tooltip on the fly
            $(me.findGroupSelect2Elem).tooltip('destroy');
            $(me.findGroupSelect2Elem).tooltip({
                title: 'group has already been added',
                placement: 'bottom',
                trigger: 'manual'
            });

            $(me.findGroupSelect2Elem).tooltip('show');
            setTimeout(function () {
                $(me.findGroupSelect2Elem).tooltip('hide');
            }, 3000);
        } else {

            // get the group model
            var groupModel = me.groupModels[selGroupId];
            me.pubSub.publish('add-group', groupModel);

            // clear the dropdown
            $(me.findGroupDropdownElem).select2('val', '');
        }
    }

    /**
     * Adds a new group to the list (unsaved)
     */
    this.addNewGroup = function (event) {
        me.pubSub.publish('add-new-group');
    }

    /**
     * Remove the given group from the list and dropdown
     */
    this.removeGroup = function (groupId) {
        delete me.groupModels[groupId];

        // remove from dropdown list
        $(me.findGroupDropdownElem).find('option[value="' + groupId + '"]').remove();
    }

    /**
     * Handle click of the save all button
     */
    this.saveAllGroups = function () {

        if (!me.savingAll) {
            me.savingAll = true; // prevent user from trigger save all while saving
            me.pubSub.publish('save-all-groups');
        }
    }

    /**
     * Handle click of the print all groups button
     */
    this.printAllGroups = function () {
        if (!me.savingAll) {
            me.pubSub.publish('print-all-groups');
        }
    }

    /**
     * Navigates back to the first screen
     */
    this.navigateBack = function () {
        me.pubSub.publish('has-dirty-groups-huh', function (isDirty) {
            if (isDirty) {
                var confirmation = confirm("There are unsaved changes. Would you still like to navigate back?");
                if (!confirmation) {
                    return;
                }
            }
            window.location = "GroupSelection";
        });
    }

    /**
     *
     */
    this.logout = function () {
        var hasDirtyGroups = student_grouping.groupListWidgetComponent.hasDirtyGroups();
        if (hasDirtyGroups) {
            var confirmDirtyLogout = confirm("You have unsaved changes. Would you still like to log out?");
            if (!confirmDirtyLogout) {
                return;
            }
        }
        me.pubSub.publish('logout');
    }

    /**
     * Hide this widget
     */
    this.toggleVisible = function (visible) {
        if (visible) {
            $(me.topbarControls).show();
        } else {
            $(me.topbarControls).hide();
        }
    }
}