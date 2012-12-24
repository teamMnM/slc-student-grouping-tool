var student_grouping = student_grouping || {};

student_grouping.groupSelectionTopbarWidget = function () {
    var me = this;
    this.pubSub = PubSub;

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
    }

    /**
     * Sets up the event handlers for user interaction with the widget
     */
    this.setupEventHandlers = function () {

        $(me.groupSearchBtnClass).click(function (event) {
            me.filterGroupsByName();
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
            if (group_selection.groupDetailsComponent.dirty) {
                var confirmDirtyLogout = confirm("You have unsaved changes. Would you still like to log out?");
                if (!confirmDirtyLogout) {
                    return;
                }
            }
            me.pubSub.publish('logout');
        });
    }

    /**************************
     * METHODS
     **************************/
    /**
     * 
     */
    this.filterGroupsByName = function () {
        var groupName = $(me.groupSearchTxtElem).select2('data').text;
        me.pubSub.publish('filter-group', groupName);
    }

    this.clearGroupSearch = function () {
        $(me.groupSearchTxtElem).select2('val', '');
        me.pubSub.publish('filter-group', '');
    }

    /**
     * Create the specified number of groups
     */
    this.createGroups = function () {
        var numGroups = $(me.numGroupsCreateTxt).val();
        window.location = "MultipleGroupsEdit?create=" + numGroups;
    }

    /**
     * 
     */
    this.editMultipleGroups = function () {
        me.pubSub.publish('edit-multiple-groups');
    }
}