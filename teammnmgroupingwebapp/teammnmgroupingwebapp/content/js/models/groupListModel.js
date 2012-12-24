var student_grouping = student_grouping || {};

/**
 * List of group models
 * @param groupModels
 */
student_grouping.groupListModel = function () {
    var me = this;

    this.groupModels = [];

    /**************************
     * METHODS
     **************************/
    /**
     * Add the given group to the list
     */
    this.addGroup = function (groupModel) {
        me.groupModels.push(groupModel);
    }

    /**
     * Adds a list of groups
     */
    this.addGroups = function (groupModels) {
        _.each(groupModels, function (groupModel) {
            me.addGroup(groupModel);
        });
    }

    /**
     * Return the groupModel with the given groupId
     */
    this.getGroupById = function (groupId) {
        var groupModel = _.find(me.groupModels, function (gm) {
            return gm.getId() === groupId;
        });
        return groupModel;
    }

    /**
     * Return the groupModel with the given name
     */
    this.getGroupByName = function (groupName) {
        var groupModel = _.find(me.groupModels, function (gm) {
            return gm.groupData.cohortIdentifier === groupName;
        });
        return groupModel;
    }
}