var student_grouping = student_grouping || {};

student_grouping.sectionListWidget = function () {
    var me = this;
    this.pubSub = PubSub;

    this.sectionModels = [];
    this.sectionWidgets = [];
    this.groupSectionClass = '.group-section';

    this.groupSectionList = '.group-section-list';
    this.editMultipleGroupsBtn = "#edit-multiple-groups-btn";

    this.newSectionId = 1;
    /**************************
     * SETUP METHODS
     **************************/
    /**
     * Initialize this widget
     */
    this.init = function (groupModels) {
        var sections = [];

        // sort the list of groups by lastModified timestamp
        var groups = _.sortBy(groupModels, function (groupModel) {
            var date = groupModel.getLastModTime();
            return parseInt(date.getDate());
        });

        // sort descending
        groups.reverse();

        _.each(groups, function (groupModel) {
            me.addGroup(groupModel);
        });

        var scrollbar = $(".group-section-div .box-wrap").antiscroll();
        $('.group-section-div .antiscroll-inner').scroll(function () {
            me.pubSub.publish('group-list-scrolled');
        });

        me.setupSubscriptions();
    }

    /**
     * Sets up listeners for pubsub events
     */
    this.setupSubscriptions = function () {

        me.pubSub.subscribe('edit-multiple-groups', me.editMultipleGroups);

        me.pubSub.subscribe('add-new-group', me.addGroup);
    }

    /**************************
     * METHODS
     **************************/
    /**
     * Add the group to the appropriate section 
     */
    this.addGroup = function (groupModel) {
        var lastModifiedDate = groupModel.getLastModTime();
        var key = lastModifiedDate.toFormat('DDDD, MMM DD, YYYY');
        var sectionModel = me.sectionModels[key];
        if (sectionModel === undefined || section === null) {
            
            sectionModel = new student_grouping.sectionModel(
                me.newSectionId++,
                "Last modified " + key,
                lastModifiedDate
            );
            me.sectionModels[key] = section;

            var sectionWidget = new student_grouping.sectionWidget(sectionModel);
            section = new group_selection.groupSection(sectionInfo);
            section.init();
            $(me.groupSectionList).append(sectionWidget.generateTemplate());
            
            me.sectionWidgets[key] = sectionWidget;
        }

        sectionModel.addGroup(groupModel);        
        var sectionWidget = me.sectionWidgets[key];
        sectionWidget.addGroup(groupModel);
    }

    /**
     *
     */
    this.editMultipleGroups = function () {
        var selGroups = [];
        for (var sectionId in me.sectionWidgets) {
            var sectionWidget = me.sectionWidgets[sectionId];
            selGroups.push.apply(selGroups, sectionWidget.getSelectedGroups());
        }

        // make sure groups have been selected
        if (selGroups.length === 0) {
            utils.uiUtils.showTooltip(
                        $(me.editMultipleGroupsBtn),
                        'Please select some groups for editing.',
                        'right',
                        'manual',
                        3000);
            return;
        }

        var selGroupIds = _.map(selGroups, function (groupWidget) {
            return groupWidget.groupModel.getId();
        });

        var selGroupIdsStr = selGroupIds.join(",");
        window.location = 'MultipleGroupsEdit?selGroups=' + selGroupIdsStr;
    }
}