﻿var student_grouping = student_grouping || {};

student_grouping.groupSelectionTopbarWidget = function () {
    var me = this;
    this.dirty = false;
    this.pubSub = PubSub;

    this.containerId = ".top-bar-controls";
    this.editMultipleGroupsBtn = "#edit-multiple-groups-btn";
    this.groupSearchTxtElem = "#group-search-txt";
    this.groupSearchBtnClass = ".group-search-btn";
    this.groupSearchClearBtnClass = ".group-search-clear-btn";
    this.createGroupsBtn = '#create-groups-btn';
    this.numGroupsCreateTxt = '.num-groups-create-txt';
    this.logoutBtnClass = '.logout-btn';

    /**************************
     * SETUP METHODS
     **************************/
    /**
     * Initialize this widget
     */
    this.init = function () {
        me.setupEventHandlers();
        me.setupSubscriptions();
    }

    /**
     * Sets up the event handlers for user interaction with the widget
     */
    this.setupEventHandlers = function () {

        // add event handler for filter student list using search box
        $(me.groupSearchTxtElem).keyup(function () {
            var filterVal = $(this).val();
            me.filterGroupsByName(filterVal);
        });

        $(me.groupSearchClearBtnClass).click(function (event) {
            me.clearGroupSearch();
        });

        $(me.createGroupsBtn).click(function (event) {
            me.createGroups();
        });

        $(me.editMultipleGroupsBtn).click(function (event) {
            me.editMultipleGroups();
        });

        $(me.logoutBtnClass).click(function (event) {
            if (student_grouping.groupDetailsWidgetComponent.dirty) {
                var confirmDirtyLogout = confirm("You have unsaved changes. Would you still like to log out?");
                if (!confirmDirtyLogout) {
                    return;
                }
            }
            me.pubSub.publish('logout');
        });
    }

    /**
     * 
     */
    this.setupSubscriptions = function () {
        me.pubSub.subscribe('toggle-dirty', me.toggleDirty);
    }

    /**************************
     * METHODS
     **************************/
    /**
     * 
     */
    this.filterGroupsByName = function (groupName) {
        me.pubSub.publish('filter-group', groupName);
    }

    /**
     * Resets the list of filtered groups
     */
    this.clearGroupSearch = function () {
        $(me.groupSearchTxtElem).val('');
        me.pubSub.publish('filter-group', '');
    }

    /**
     * Create the specified number of groups
     */
    this.createGroups = function () {
        // warn user about unsaved changes, if any
        if (me.dirty) {
            var confirm = window.confirm("You have unsaved changes. If you continue these changes will be lost. Continue?");
            if (!confirm) {
                return;
            }
        }
        var numGroups = $(me.numGroupsCreateTxt).val();
        window.location = "MultipleGroupsEdit?create=" + numGroups;
    }

    /**
     * Handle editMultipleGroups btn click event
     */
    this.editMultipleGroups = function () {
        // warn user about unsaved changes, if any
        if (me.dirty) {
            var confirm = window.confirm("You have unsaved changes. If you continue these changes will be lost. Continue?");
            if (!confirm) {
                return;
            }
        }
        me.pubSub.publish('edit-multiple-groups');
    }
    
    /**
     * Show/hide the toolbar
     */
    this.toggleVisible = function (visible) {
        if (visible) {
            $(me.containerId).show();
        } else {
            $(me.containerId).hide();
        }
    }

    /**
     * Toggle the dirty state
     */
    this.toggleDirty = function (dirty) {
        me.dirty = dirty;
    }
}