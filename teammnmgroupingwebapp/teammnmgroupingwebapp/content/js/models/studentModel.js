var student_grouping = student_grouping || {};

/**
 * Client side student model
 * @param studentData - Server side student model
 */
student_grouping.studentModel = function (studentData) {
    var me = this;
    this.pubSub = PubSub;

    this.serverStudent = studentData;

    /**************************
     * GETTER AND SETTERS
     **************************/
    /**
     * Returns the student id
     */
    this.getId = function () {
        return me.serverStudent.id;
    }

    /**
     * Returns the student name
     */
    this.getName = function () {
        return me.serverStudent.name;
    }

    /**
     * Returns the student's gender
     */
    this.getGender = function () {
        return me.serverStudent.sex;
    }

    /**
     * Generic getter for student properties, 
     * quick workaround to avoid mapping all server-side attributes
     */
    this.getProp = function (propName) {
        // TODO null and sanity check
        return me.serverStudent[propName];
    }
}