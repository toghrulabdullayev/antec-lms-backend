using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AntecLMS.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SnakeCaseHistoryFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_attendances_lessons_LessonId",
                table: "attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_attendances_students_StudentId",
                table: "attendances");

            migrationBuilder.DropForeignKey(
                name: "FK_grades_lessons_LessonId",
                table: "grades");

            migrationBuilder.DropForeignKey(
                name: "FK_grades_students_StudentId",
                table: "grades");

            migrationBuilder.DropForeignKey(
                name: "FK_group_students_groups_GroupId",
                table: "group_students");

            migrationBuilder.DropForeignKey(
                name: "FK_group_students_students_StudentId",
                table: "group_students");

            migrationBuilder.DropForeignKey(
                name: "FK_groups_courses_CourseId",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "FK_groups_teachers_TeacherId",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "FK_lessons_groups_GroupId",
                table: "lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_lessons_teachers_TeacherId",
                table: "lessons");

            migrationBuilder.DropForeignKey(
                name: "FK_materials_groups_GroupId",
                table: "materials");

            migrationBuilder.DropForeignKey(
                name: "FK_materials_lessons_LessonId",
                table: "materials");

            migrationBuilder.DropForeignKey(
                name: "FK_materials_teachers_TeacherId",
                table: "materials");

            migrationBuilder.DropForeignKey(
                name: "FK_students_users_UserId",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "FK_teachers_users_UserId",
                table: "teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_teachers",
                table: "teachers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_students",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_materials",
                table: "materials");

            migrationBuilder.DropPrimaryKey(
                name: "PK_lessons",
                table: "lessons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_groups",
                table: "groups");

            migrationBuilder.DropPrimaryKey(
                name: "PK_group_students",
                table: "group_students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_grades",
                table: "grades");

            migrationBuilder.DropPrimaryKey(
                name: "PK_courses",
                table: "courses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_attendances",
                table: "attendances");

            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "users",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "teachers",
                newName: "teachers",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "students",
                newName: "students",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "materials",
                newName: "materials",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "lessons",
                newName: "lessons",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "groups",
                newName: "groups",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "group_students",
                newName: "group_students",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "grades",
                newName: "grades",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "courses",
                newName: "courses",
                newSchema: "public");

            migrationBuilder.RenameTable(
                name: "attendances",
                newName: "attendances",
                newSchema: "public");

            migrationBuilder.RenameColumn(
                name: "Surname",
                schema: "public",
                table: "users",
                newName: "surname");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "public",
                table: "users",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Role",
                schema: "public",
                table: "users",
                newName: "role");

            migrationBuilder.RenameColumn(
                name: "Phone",
                schema: "public",
                table: "users",
                newName: "phone");

            migrationBuilder.RenameColumn(
                name: "Password",
                schema: "public",
                table: "users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "public",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                schema: "public",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "users",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_users_Email",
                schema: "public",
                table: "users",
                newName: "ix_users_email");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "public",
                table: "teachers",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Specialization",
                schema: "public",
                table: "teachers",
                newName: "specialization");

            migrationBuilder.RenameColumn(
                name: "Bio",
                schema: "public",
                table: "teachers",
                newName: "bio");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "teachers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "public",
                table: "teachers",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "teachers",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "teachers",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_teachers_UserId",
                schema: "public",
                table: "teachers",
                newName: "ix_teachers_user_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "public",
                table: "students",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Note",
                schema: "public",
                table: "students",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "students",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "public",
                table: "students",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "students",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "students",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "BirthDate",
                schema: "public",
                table: "students",
                newName: "birth_date");

            migrationBuilder.RenameIndex(
                name: "IX_students_UserId",
                schema: "public",
                table: "students",
                newName: "ix_students_user_id");

            migrationBuilder.RenameColumn(
                name: "Url",
                schema: "public",
                table: "materials",
                newName: "url");

            migrationBuilder.RenameColumn(
                name: "Type",
                schema: "public",
                table: "materials",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "public",
                table: "materials",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "public",
                table: "materials",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "materials",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "materials",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                schema: "public",
                table: "materials",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                schema: "public",
                table: "materials",
                newName: "lesson_id");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                schema: "public",
                table: "materials",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                schema: "public",
                table: "materials",
                newName: "file_path");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "materials",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_materials_TeacherId",
                schema: "public",
                table: "materials",
                newName: "ix_materials_teacher_id");

            migrationBuilder.RenameIndex(
                name: "IX_materials_LessonId",
                schema: "public",
                table: "materials",
                newName: "ix_materials_lesson_id");

            migrationBuilder.RenameIndex(
                name: "IX_materials_GroupId",
                schema: "public",
                table: "materials",
                newName: "ix_materials_group_id");

            migrationBuilder.RenameColumn(
                name: "Topic",
                schema: "public",
                table: "lessons",
                newName: "topic");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "public",
                table: "lessons",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Note",
                schema: "public",
                table: "lessons",
                newName: "note");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "lessons",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "lessons",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                schema: "public",
                table: "lessons",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "LessonDate",
                schema: "public",
                table: "lessons",
                newName: "lesson_date");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                schema: "public",
                table: "lessons",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "lessons",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_lessons_TeacherId",
                schema: "public",
                table: "lessons",
                newName: "ix_lessons_teacher_id");

            migrationBuilder.RenameIndex(
                name: "IX_lessons_GroupId",
                schema: "public",
                table: "lessons",
                newName: "ix_lessons_group_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "public",
                table: "groups",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "public",
                table: "groups",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "groups",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "groups",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TeacherId",
                schema: "public",
                table: "groups",
                newName: "teacher_id");

            migrationBuilder.RenameColumn(
                name: "StartDate",
                schema: "public",
                table: "groups",
                newName: "start_date");

            migrationBuilder.RenameColumn(
                name: "EndDate",
                schema: "public",
                table: "groups",
                newName: "end_date");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "groups",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                schema: "public",
                table: "groups",
                newName: "course_id");

            migrationBuilder.RenameIndex(
                name: "IX_groups_TeacherId",
                schema: "public",
                table: "groups",
                newName: "ix_groups_teacher_id");

            migrationBuilder.RenameIndex(
                name: "IX_groups_CourseId",
                schema: "public",
                table: "groups",
                newName: "ix_groups_course_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "public",
                table: "group_students",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "group_students",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "group_students",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "public",
                table: "group_students",
                newName: "student_id");

            migrationBuilder.RenameColumn(
                name: "JoinedAt",
                schema: "public",
                table: "group_students",
                newName: "joined_at");

            migrationBuilder.RenameColumn(
                name: "GroupId",
                schema: "public",
                table: "group_students",
                newName: "group_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "group_students",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_group_students_StudentId",
                schema: "public",
                table: "group_students",
                newName: "ix_group_students_student_id");

            migrationBuilder.RenameIndex(
                name: "IX_group_students_GroupId_StudentId",
                schema: "public",
                table: "group_students",
                newName: "ix_group_students_group_id_student_id");

            migrationBuilder.RenameColumn(
                name: "Score",
                schema: "public",
                table: "grades",
                newName: "score");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "grades",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "grades",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TeacherNote",
                schema: "public",
                table: "grades",
                newName: "teacher_note");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "public",
                table: "grades",
                newName: "student_id");

            migrationBuilder.RenameColumn(
                name: "MaxScore",
                schema: "public",
                table: "grades",
                newName: "max_score");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                schema: "public",
                table: "grades",
                newName: "lesson_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "grades",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_grades_StudentId",
                schema: "public",
                table: "grades",
                newName: "ix_grades_student_id");

            migrationBuilder.RenameIndex(
                name: "IX_grades_LessonId",
                schema: "public",
                table: "grades",
                newName: "ix_grades_lesson_id");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "public",
                table: "courses",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Price",
                schema: "public",
                table: "courses",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                schema: "public",
                table: "courses",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "public",
                table: "courses",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "courses",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "courses",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                schema: "public",
                table: "courses",
                newName: "image_url");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "courses",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "public",
                table: "attendances",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Reason",
                schema: "public",
                table: "attendances",
                newName: "reason");

            migrationBuilder.RenameColumn(
                name: "Id",
                schema: "public",
                table: "attendances",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                schema: "public",
                table: "attendances",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "TeacherNote",
                schema: "public",
                table: "attendances",
                newName: "teacher_note");

            migrationBuilder.RenameColumn(
                name: "StudentId",
                schema: "public",
                table: "attendances",
                newName: "student_id");

            migrationBuilder.RenameColumn(
                name: "MinutesLate",
                schema: "public",
                table: "attendances",
                newName: "minutes_late");

            migrationBuilder.RenameColumn(
                name: "LessonId",
                schema: "public",
                table: "attendances",
                newName: "lesson_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                schema: "public",
                table: "attendances",
                newName: "created_at");

            migrationBuilder.RenameIndex(
                name: "IX_attendances_StudentId",
                schema: "public",
                table: "attendances",
                newName: "ix_attendances_student_id");

            migrationBuilder.RenameIndex(
                name: "IX_attendances_LessonId_StudentId",
                schema: "public",
                table: "attendances",
                newName: "ix_attendances_lesson_id_student_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_users",
                schema: "public",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_teachers",
                schema: "public",
                table: "teachers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_students",
                schema: "public",
                table: "students",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_materials",
                schema: "public",
                table: "materials",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_lessons",
                schema: "public",
                table: "lessons",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_groups",
                schema: "public",
                table: "groups",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_group_students",
                schema: "public",
                table: "group_students",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_grades",
                schema: "public",
                table: "grades",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_courses",
                schema: "public",
                table: "courses",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_attendances",
                schema: "public",
                table: "attendances",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_attendances_lessons_lesson_id",
                schema: "public",
                table: "attendances",
                column: "lesson_id",
                principalSchema: "public",
                principalTable: "lessons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_attendances_students_student_id",
                schema: "public",
                table: "attendances",
                column: "student_id",
                principalSchema: "public",
                principalTable: "students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_grades_lessons_lesson_id",
                schema: "public",
                table: "grades",
                column: "lesson_id",
                principalSchema: "public",
                principalTable: "lessons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_grades_students_student_id",
                schema: "public",
                table: "grades",
                column: "student_id",
                principalSchema: "public",
                principalTable: "students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_group_students_groups_group_id",
                schema: "public",
                table: "group_students",
                column: "group_id",
                principalSchema: "public",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_group_students_students_student_id",
                schema: "public",
                table: "group_students",
                column: "student_id",
                principalSchema: "public",
                principalTable: "students",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_groups_courses_course_id",
                schema: "public",
                table: "groups",
                column: "course_id",
                principalSchema: "public",
                principalTable: "courses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_groups_teachers_teacher_id",
                schema: "public",
                table: "groups",
                column: "teacher_id",
                principalSchema: "public",
                principalTable: "teachers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_lessons_groups_group_id",
                schema: "public",
                table: "lessons",
                column: "group_id",
                principalSchema: "public",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_lessons_teachers_teacher_id",
                schema: "public",
                table: "lessons",
                column: "teacher_id",
                principalSchema: "public",
                principalTable: "teachers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_materials_groups_group_id",
                schema: "public",
                table: "materials",
                column: "group_id",
                principalSchema: "public",
                principalTable: "groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_materials_lessons_lesson_id",
                schema: "public",
                table: "materials",
                column: "lesson_id",
                principalSchema: "public",
                principalTable: "lessons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_materials_teachers_teacher_id",
                schema: "public",
                table: "materials",
                column: "teacher_id",
                principalSchema: "public",
                principalTable: "teachers",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_students_users_user_id",
                schema: "public",
                table: "students",
                column: "user_id",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_teachers_users_user_id",
                schema: "public",
                table: "teachers",
                column: "user_id",
                principalSchema: "public",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_attendances_lessons_lesson_id",
                schema: "public",
                table: "attendances");

            migrationBuilder.DropForeignKey(
                name: "fk_attendances_students_student_id",
                schema: "public",
                table: "attendances");

            migrationBuilder.DropForeignKey(
                name: "fk_grades_lessons_lesson_id",
                schema: "public",
                table: "grades");

            migrationBuilder.DropForeignKey(
                name: "fk_grades_students_student_id",
                schema: "public",
                table: "grades");

            migrationBuilder.DropForeignKey(
                name: "fk_group_students_groups_group_id",
                schema: "public",
                table: "group_students");

            migrationBuilder.DropForeignKey(
                name: "fk_group_students_students_student_id",
                schema: "public",
                table: "group_students");

            migrationBuilder.DropForeignKey(
                name: "fk_groups_courses_course_id",
                schema: "public",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "fk_groups_teachers_teacher_id",
                schema: "public",
                table: "groups");

            migrationBuilder.DropForeignKey(
                name: "fk_lessons_groups_group_id",
                schema: "public",
                table: "lessons");

            migrationBuilder.DropForeignKey(
                name: "fk_lessons_teachers_teacher_id",
                schema: "public",
                table: "lessons");

            migrationBuilder.DropForeignKey(
                name: "fk_materials_groups_group_id",
                schema: "public",
                table: "materials");

            migrationBuilder.DropForeignKey(
                name: "fk_materials_lessons_lesson_id",
                schema: "public",
                table: "materials");

            migrationBuilder.DropForeignKey(
                name: "fk_materials_teachers_teacher_id",
                schema: "public",
                table: "materials");

            migrationBuilder.DropForeignKey(
                name: "fk_students_users_user_id",
                schema: "public",
                table: "students");

            migrationBuilder.DropForeignKey(
                name: "fk_teachers_users_user_id",
                schema: "public",
                table: "teachers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_users",
                schema: "public",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_teachers",
                schema: "public",
                table: "teachers");

            migrationBuilder.DropPrimaryKey(
                name: "pk_students",
                schema: "public",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "pk_materials",
                schema: "public",
                table: "materials");

            migrationBuilder.DropPrimaryKey(
                name: "pk_lessons",
                schema: "public",
                table: "lessons");

            migrationBuilder.DropPrimaryKey(
                name: "pk_groups",
                schema: "public",
                table: "groups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_group_students",
                schema: "public",
                table: "group_students");

            migrationBuilder.DropPrimaryKey(
                name: "pk_grades",
                schema: "public",
                table: "grades");

            migrationBuilder.DropPrimaryKey(
                name: "pk_courses",
                schema: "public",
                table: "courses");

            migrationBuilder.DropPrimaryKey(
                name: "pk_attendances",
                schema: "public",
                table: "attendances");

            migrationBuilder.RenameTable(
                name: "users",
                schema: "public",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "teachers",
                schema: "public",
                newName: "teachers");

            migrationBuilder.RenameTable(
                name: "students",
                schema: "public",
                newName: "students");

            migrationBuilder.RenameTable(
                name: "materials",
                schema: "public",
                newName: "materials");

            migrationBuilder.RenameTable(
                name: "lessons",
                schema: "public",
                newName: "lessons");

            migrationBuilder.RenameTable(
                name: "groups",
                schema: "public",
                newName: "groups");

            migrationBuilder.RenameTable(
                name: "group_students",
                schema: "public",
                newName: "group_students");

            migrationBuilder.RenameTable(
                name: "grades",
                schema: "public",
                newName: "grades");

            migrationBuilder.RenameTable(
                name: "courses",
                schema: "public",
                newName: "courses");

            migrationBuilder.RenameTable(
                name: "attendances",
                schema: "public",
                newName: "attendances");

            migrationBuilder.RenameColumn(
                name: "surname",
                table: "users",
                newName: "Surname");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "users",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "role",
                table: "users",
                newName: "Role");

            migrationBuilder.RenameColumn(
                name: "phone",
                table: "users",
                newName: "Phone");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "users",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "users",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_users_email",
                table: "users",
                newName: "IX_users_Email");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "teachers",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "specialization",
                table: "teachers",
                newName: "Specialization");

            migrationBuilder.RenameColumn(
                name: "bio",
                table: "teachers",
                newName: "Bio");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "teachers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "teachers",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "teachers",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "teachers",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_teachers_user_id",
                table: "teachers",
                newName: "IX_teachers_UserId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "students",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "students",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "students",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "students",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "students",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "students",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "birth_date",
                table: "students",
                newName: "BirthDate");

            migrationBuilder.RenameIndex(
                name: "ix_students_user_id",
                table: "students",
                newName: "IX_students_UserId");

            migrationBuilder.RenameColumn(
                name: "url",
                table: "materials",
                newName: "Url");

            migrationBuilder.RenameColumn(
                name: "type",
                table: "materials",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "title",
                table: "materials",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "materials",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "materials",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "materials",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "materials",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "lesson_id",
                table: "materials",
                newName: "LessonId");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "materials",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "file_path",
                table: "materials",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "materials",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_materials_teacher_id",
                table: "materials",
                newName: "IX_materials_TeacherId");

            migrationBuilder.RenameIndex(
                name: "ix_materials_lesson_id",
                table: "materials",
                newName: "IX_materials_LessonId");

            migrationBuilder.RenameIndex(
                name: "ix_materials_group_id",
                table: "materials",
                newName: "IX_materials_GroupId");

            migrationBuilder.RenameColumn(
                name: "topic",
                table: "lessons",
                newName: "Topic");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "lessons",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "lessons",
                newName: "Note");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "lessons",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "lessons",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "lessons",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "lesson_date",
                table: "lessons",
                newName: "LessonDate");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "lessons",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "lessons",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_lessons_teacher_id",
                table: "lessons",
                newName: "IX_lessons_TeacherId");

            migrationBuilder.RenameIndex(
                name: "ix_lessons_group_id",
                table: "lessons",
                newName: "IX_lessons_GroupId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "groups",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "groups",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "groups",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "groups",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "teacher_id",
                table: "groups",
                newName: "TeacherId");

            migrationBuilder.RenameColumn(
                name: "start_date",
                table: "groups",
                newName: "StartDate");

            migrationBuilder.RenameColumn(
                name: "end_date",
                table: "groups",
                newName: "EndDate");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "groups",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "course_id",
                table: "groups",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "ix_groups_teacher_id",
                table: "groups",
                newName: "IX_groups_TeacherId");

            migrationBuilder.RenameIndex(
                name: "ix_groups_course_id",
                table: "groups",
                newName: "IX_groups_CourseId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "group_students",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "group_students",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "group_students",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "group_students",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "joined_at",
                table: "group_students",
                newName: "JoinedAt");

            migrationBuilder.RenameColumn(
                name: "group_id",
                table: "group_students",
                newName: "GroupId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "group_students",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_group_students_student_id",
                table: "group_students",
                newName: "IX_group_students_StudentId");

            migrationBuilder.RenameIndex(
                name: "ix_group_students_group_id_student_id",
                table: "group_students",
                newName: "IX_group_students_GroupId_StudentId");

            migrationBuilder.RenameColumn(
                name: "score",
                table: "grades",
                newName: "Score");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "grades",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "grades",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "teacher_note",
                table: "grades",
                newName: "TeacherNote");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "grades",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "max_score",
                table: "grades",
                newName: "MaxScore");

            migrationBuilder.RenameColumn(
                name: "lesson_id",
                table: "grades",
                newName: "LessonId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "grades",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_grades_student_id",
                table: "grades",
                newName: "IX_grades_StudentId");

            migrationBuilder.RenameIndex(
                name: "ix_grades_lesson_id",
                table: "grades",
                newName: "IX_grades_LessonId");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "courses",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "courses",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "courses",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "courses",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "courses",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "courses",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "image_url",
                table: "courses",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "courses",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "attendances",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "reason",
                table: "attendances",
                newName: "Reason");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "attendances",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "attendances",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "teacher_note",
                table: "attendances",
                newName: "TeacherNote");

            migrationBuilder.RenameColumn(
                name: "student_id",
                table: "attendances",
                newName: "StudentId");

            migrationBuilder.RenameColumn(
                name: "minutes_late",
                table: "attendances",
                newName: "MinutesLate");

            migrationBuilder.RenameColumn(
                name: "lesson_id",
                table: "attendances",
                newName: "LessonId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "attendances",
                newName: "CreatedAt");

            migrationBuilder.RenameIndex(
                name: "ix_attendances_student_id",
                table: "attendances",
                newName: "IX_attendances_StudentId");

            migrationBuilder.RenameIndex(
                name: "ix_attendances_lesson_id_student_id",
                table: "attendances",
                newName: "IX_attendances_LessonId_StudentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_teachers",
                table: "teachers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_students",
                table: "students",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_materials",
                table: "materials",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_lessons",
                table: "lessons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_groups",
                table: "groups",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_group_students",
                table: "group_students",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_grades",
                table: "grades",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_courses",
                table: "courses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_attendances",
                table: "attendances",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_attendances_lessons_LessonId",
                table: "attendances",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_attendances_students_StudentId",
                table: "attendances",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_grades_lessons_LessonId",
                table: "grades",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_grades_students_StudentId",
                table: "grades",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_students_groups_GroupId",
                table: "group_students",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_group_students_students_StudentId",
                table: "group_students",
                column: "StudentId",
                principalTable: "students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_groups_courses_CourseId",
                table: "groups",
                column: "CourseId",
                principalTable: "courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_groups_teachers_TeacherId",
                table: "groups",
                column: "TeacherId",
                principalTable: "teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_lessons_groups_GroupId",
                table: "lessons",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_lessons_teachers_TeacherId",
                table: "lessons",
                column: "TeacherId",
                principalTable: "teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_materials_groups_GroupId",
                table: "materials",
                column: "GroupId",
                principalTable: "groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_materials_lessons_LessonId",
                table: "materials",
                column: "LessonId",
                principalTable: "lessons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_materials_teachers_TeacherId",
                table: "materials",
                column: "TeacherId",
                principalTable: "teachers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_students_users_UserId",
                table: "students",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_teachers_users_UserId",
                table: "teachers",
                column: "UserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
