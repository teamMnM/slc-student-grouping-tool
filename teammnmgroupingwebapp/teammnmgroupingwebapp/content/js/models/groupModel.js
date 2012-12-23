var student_grouping = student_grouping || {};

/**
 * Client side group model
 * @param groupData - Server side group model
 */
student_grouping.groupModel = function (groupData) {
    
    var me = this;
    this.pubSub = PubSub;

    this.serverGroup = groupData;
    this.groupData = groupData.cohort;
    this.students = [];
    this.selectedAttributes = [];
    this.attachedFile = null;

    /**************************
     * GETTER AND SETTERS
     **************************/
    /**
     * Returns the cohort id
     */
    this.getId = function () {
        return me.groupData.id;
    }

    /**
     * Set the id of the group to the given id
     */
    this.setId = function (id) {
        me.groupData.id = id;
    }

    /**
     * Returns the custom object
     */
    this.getCustom = function () {
        return me.serverGroup.custom;
    }

    /**
     * Returns an array containing the students in the original list (sent from server)
     */
    this.getOriginalStudents = function () {
        return me.serverGroup.students;
    }

    /**************************
     * METHODS
     **************************/
    /**
     * Returns true if student is already part of group
     */
    this.hasStudent = function (studentId) {
        var student = _.find(me.students, function (s) {
            return s === studentId;
        });
        return student !== undefined;
    }

    /**
     * Add given student to list of student
     */
    this.addStudent = function (studentId) {
        me.students.push(studentId);
    }

    /**
     * Removes the given student from the list
     */
    this.removeStudent = function (studentId) {
        me.students = _.filter(me.students, function (id) {
            return id !== studentId;
        });
    }

    /**
     * Returns true if there is no attached file
     */
    this.hasAttachedFile = function () {
        return me.attachedFile !== null;
    }

    /**
     * Attaches the given file to the group
     */
    this.attachFile = function (file) {
        me.attachedFile = file;
    }

    /**
     * Removes this group's file attachment
     */
    this.removeAttachedFile = function () {
        me.attachedFile = null;
    }

    /**
     * Returns true if this group is a new group. 
     * A new group has a negative id
     */
    this.isNewGroup = function () {
        return parseInt(me.groupData.id) < 0;
    }

    /**
     * Add new group or update a group
     */
    this.saveGroupChanges = function (successHandler, errorHandler) {

        var cohortActionObject = me.prepareGroupForSaving();

        // negative ids represent new groups
        var method = 'CreateGroup';
        var successHandler = successHandler;
        var errorHandler = errorHandler;
        if (me.isNewGroup()) {
            cohortActionObject.cohort.id = null;
        } else {
            method = 'UpdateGroup';
        }

        $.ajax({
            type: 'POST',
            url: method,
            contentType: 'application/json',
            data: JSON.stringify(cohortActionObject),
            success: function (result) {

                me.updateStudentList();
                
                if (result.completedSuccessfully) {
                    successHandler(result);
                } else if (!result.partialCreateSuccess || !result.partialDeleteSuccess || !result.customActionResult.isSuccess) {
                    errorHandler(result);
                }
            },
            error: function (result) {
                // should implement exception handling
            }
        });
        me.toggleGroupContainerProcessingState(true);
    }

    /**
     * Creates JSON object to send back to server for saving changes
     */
    this.prepareGroupForSaving = function () {
       
        var newStudents = me.getStudentsToCreate()
        var studentsToDelete = me.getStudentsToDelete();
        
        // if negative, then it is a new group so it doesn't have an id.
        // server expects null for new groups
        var id = me.isNewGroup() ?
             null : me.groupData.id;

        var cohortActionObject = {
            cohort: {
                id: id,
                cohortDescription: me.groupData.cohortDescription,
                cohortIdentifier: me.groupData.cohortIdentifier
            },
            custom: { dataElements: me.selectedAttributes, lessonPlan: me.attachedFile },
            studentsToDelete: studentsToDelete !== null ? studentsToDelete : [],
            studentsToCreate: newStudents !== null ? newStudents : []
        }

        return cohortActionObject;
    }

    /**
     * Returns the new students. Determined by new students minus old students
     */
    this.getStudentsToCreate = function () {
        var originalStudents = me.serverGroup.students;
        var newStudents = _.filter(me.students, function (student) {
            var matchingStudent = _.find(originalStudents, function (origStudentId) {
                return origStudentId === student;
            });
            return matchingStudent === undefined;
        });

        // remove blanks
        newStudents = _.filter(newStudents, function (student) {
            return student !== "";
        });

        return newStudents;
    }

    /**
     * Returns the students to delete from this group. Determined by old 
     * students minus new students
     */
    this.getStudentsToDelete = function () {
        var studentsToDelete = _.filter(me.originalStudents, function (origStudentId) {
            var matchingStudent = _.find(me.students, function (student) {
                return origStudentId === student;
            });
            return matchingStudent === undefined;
        });

        // remove blanks
        studentsToDelete = _.filter(studentsToDelete, function (student) {
            return student !== "";
        });

        return studentsToDelete;
    }

    /**
     * TODO add description
     * @param failedCreates - failed to create new student associations
     * @param failedDeletes - failed to delete student associations
     */
    this.updateStudentList = function (failedCreates, failedDeletes) {
        var failedCreates = [];
        if (result.failToCreateAssociations !== null) {
            failedCreates = _.pluck(result.failToCreateAssociations, 'objectId');
        }
        me.updateListWithNewStudents(failedCreates);

        var failedDeletes = [];
        if (result.failToDeleAssociations !== null) {
            _.pluck(result.failToDeleteAssociations, 'objectId');
        }
        me.updateListWithDeletedStudents(failedDeletes);

    }

    /**
     * Adds the newly created students to the original list of students
     * @param failedStudentIds - ids of students that could not be added to the group in the backend
     */
    this.updateListWithNewStudents = function (failedStudentIds) {
        var studentsToCreate = me.getStudentsToCreate();

        // filter out the failed creates
        _.each(studentsToCreate, function (studentToCreate) {
            var matchingStudent = _.find(failedStudentIds, function (failedStudentId) {
                return failedStudentId === studentToCreate;
            });

            if (matchingStudent === undefined) {
                // student was successfully created in backend
                me.serverGroup.students.push(studentToCreate);
            }
        });
    }

    /**
     * Removes the deleted students from the original list of students
     * @param failedStudentIds - ids of the students that could not be deleted from grop in the backend
     */
    this.updateListWithDeletedStudents = function (failedStudentIds) {
        // filter out unsuccessful deletes
        var studentsToDelete = me.getStudentsToDelete();
        _.each(studentsToDelete, function (studentToDel) {
            var matchingStudent = _.find(failedStudentIds, function (failedStudentId) {
                return failedStudentId === studentToDel;
            });

            if (matchingStudent === undefined) {
                // student was successfully deleted in backend
                me.serverGroup.students = _.filter(me.serverGroup.students, function (originalStudent) {
                    return originalStudent !== studentToDel;
                });
            }
        });
    }
}