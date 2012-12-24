var student_grouping = student_grouping || {};

/**
 * Client side group model
 * @param groupData - Server side group model
 */
student_grouping.sectionModel = function (id, title, date) {
    var me = this;
    this.pubSub = PubSub;

    this.id = id;
    this.title = title;
    this.date = date;

    this.groupModels = [];

    /**************************
     * GETTER AND SETTERS
     **************************/
    /**
     * Returns the cohort id
     */
    this.getId = function () {
        return me.id;
    }

    /**************************
    * METHODS
    **************************/

    /**
     * Add the given group to this section
     */
    this.addGroup = function (groupModel) {
        me.groupModels.push(groupModel);
    }

    /**
     * Delete the given group from this section
     */
    this.removeGroup = function (groupId) {
        me.groupModels = _.filter(me.groupModels, function (groupModel) {
            return groupModel.getId() !== groupId;
        });
    }

    /**
     * Returns the group with the given id
     */
    this.getGroupById = function (groupId) {
        return _.find(me.groupModels, function (groupModel) {
            return groupModel.getId() === groupId;
        });
    }

    /**
     * Returns true if this section contains the given group
     */
    this.hasGroup = function (groupId) {
        var groupModel = me.getGroupById(groupId);
        return groupModel !== undefined && groupModel !== null;
    }

    /**
     *
     */
    this.getGroupByIndex = function (index) {
        return me.groupModels[0];
    }
}