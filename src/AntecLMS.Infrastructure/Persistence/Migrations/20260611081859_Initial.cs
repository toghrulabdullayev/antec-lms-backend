using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AntecLMS.Infrastructure.Persistence.Migrations
{
  /// <inheritdoc />
  public partial class Initial : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: "courses",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          name = table.Column<string>(
            type: "character varying(200)",
            maxLength: 200,
            nullable: false
          ),
          description = table.Column<string>(
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: true
          ),
          price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
          image_url = table.Column<string>(
            type: "character varying(500)",
            maxLength: 500,
            nullable: true
          ),
          status = table.Column<string>(type: "text", nullable: false),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_courses", x => x.id);
        }
      );

      migrationBuilder.CreateTable(
        name: "users",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          name = table.Column<string>(
            type: "character varying(100)",
            maxLength: 100,
            nullable: false
          ),
          surname = table.Column<string>(
            type: "character varying(100)",
            maxLength: 100,
            nullable: false
          ),
          email = table.Column<string>(
            type: "character varying(200)",
            maxLength: 200,
            nullable: false
          ),
          password = table.Column<string>(type: "text", nullable: false),
          phone = table.Column<string>(
            type: "character varying(20)",
            maxLength: 20,
            nullable: true
          ),
          role = table.Column<string>(type: "text", nullable: false),
          status = table.Column<string>(type: "text", nullable: false),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_users", x => x.id);
        }
      );

      migrationBuilder.CreateTable(
        name: "students",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          user_id = table.Column<int>(type: "integer", nullable: false),
          birth_date = table.Column<DateOnly>(type: "date", nullable: true),
          note = table.Column<string>(
            type: "character varying(500)",
            maxLength: 500,
            nullable: true
          ),
          status = table.Column<string>(type: "text", nullable: false),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_students", x => x.id);
          table.ForeignKey(
            name: "fk_students_users_user_id",
            column: x => x.user_id,
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "teachers",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          user_id = table.Column<int>(type: "integer", nullable: false),
          specialization = table.Column<string>(
            type: "character varying(500)",
            maxLength: 500,
            nullable: true
          ),
          bio = table.Column<string>(
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: true
          ),
          status = table.Column<string>(type: "text", nullable: false),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_teachers", x => x.id);
          table.ForeignKey(
            name: "fk_teachers_users_user_id",
            column: x => x.user_id,
            principalTable: "users",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "groups",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          name = table.Column<string>(
            type: "character varying(200)",
            maxLength: 200,
            nullable: false
          ),
          course_id = table.Column<int>(type: "integer", nullable: false),
          teacher_id = table.Column<int>(type: "integer", nullable: false),
          start_date = table.Column<DateOnly>(type: "date", nullable: false),
          end_date = table.Column<DateOnly>(type: "date", nullable: true),
          status = table.Column<string>(type: "text", nullable: false),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_groups", x => x.id);
          table.ForeignKey(
            name: "fk_groups_courses_course_id",
            column: x => x.course_id,
            principalTable: "courses",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict
          );
          table.ForeignKey(
            name: "fk_groups_teachers_teacher_id",
            column: x => x.teacher_id,
            principalTable: "teachers",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "group_students",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          group_id = table.Column<int>(type: "integer", nullable: false),
          student_id = table.Column<int>(type: "integer", nullable: false),
          joined_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          status = table.Column<string>(
            type: "character varying(50)",
            maxLength: 50,
            nullable: false
          ),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_group_students", x => x.id);
          table.ForeignKey(
            name: "fk_group_students_groups_group_id",
            column: x => x.group_id,
            principalTable: "groups",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "fk_group_students_students_student_id",
            column: x => x.student_id,
            principalTable: "students",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "lessons",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          group_id = table.Column<int>(type: "integer", nullable: false),
          teacher_id = table.Column<int>(type: "integer", nullable: false),
          lesson_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          topic = table.Column<string>(
            type: "character varying(300)",
            maxLength: 300,
            nullable: true
          ),
          note = table.Column<string>(
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: true
          ),
          status = table.Column<string>(
            type: "character varying(50)",
            maxLength: 50,
            nullable: false
          ),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_lessons", x => x.id);
          table.ForeignKey(
            name: "fk_lessons_groups_group_id",
            column: x => x.group_id,
            principalTable: "groups",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "fk_lessons_teachers_teacher_id",
            column: x => x.teacher_id,
            principalTable: "teachers",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "attendances",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          lesson_id = table.Column<int>(type: "integer", nullable: false),
          student_id = table.Column<int>(type: "integer", nullable: false),
          status = table.Column<string>(
            type: "character varying(50)",
            maxLength: 50,
            nullable: false
          ),
          minutes_late = table.Column<int>(type: "integer", nullable: true),
          reason = table.Column<string>(
            type: "character varying(500)",
            maxLength: 500,
            nullable: true
          ),
          teacher_note = table.Column<string>(
            type: "character varying(500)",
            maxLength: 500,
            nullable: true
          ),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_attendances", x => x.id);
          table.ForeignKey(
            name: "fk_attendances_lessons_lesson_id",
            column: x => x.lesson_id,
            principalTable: "lessons",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "fk_attendances_students_student_id",
            column: x => x.student_id,
            principalTable: "students",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "grades",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          lesson_id = table.Column<int>(type: "integer", nullable: false),
          student_id = table.Column<int>(type: "integer", nullable: false),
          score = table.Column<int>(type: "integer", nullable: false),
          max_score = table.Column<int>(type: "integer", nullable: false),
          teacher_note = table.Column<string>(
            type: "character varying(500)",
            maxLength: 500,
            nullable: true
          ),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_grades", x => x.id);
          table.ForeignKey(
            name: "fk_grades_lessons_lesson_id",
            column: x => x.lesson_id,
            principalTable: "lessons",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "fk_grades_students_student_id",
            column: x => x.student_id,
            principalTable: "students",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "materials",
        columns: table => new
        {
          id = table
            .Column<int>(type: "integer", nullable: false)
            .Annotation(
              "Npgsql:ValueGenerationStrategy",
              NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
            ),
          lesson_id = table.Column<int>(type: "integer", nullable: false),
          group_id = table.Column<int>(type: "integer", nullable: false),
          teacher_id = table.Column<int>(type: "integer", nullable: false),
          title = table.Column<string>(
            type: "character varying(300)",
            maxLength: 300,
            nullable: false
          ),
          type = table.Column<string>(
            type: "character varying(100)",
            maxLength: 100,
            nullable: true
          ),
          url = table.Column<string>(
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: true
          ),
          file_path = table.Column<string>(
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: true
          ),
          description = table.Column<string>(
            type: "character varying(1000)",
            maxLength: 1000,
            nullable: true
          ),
          created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
          updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
        },
        constraints: table =>
        {
          table.PrimaryKey("pk_materials", x => x.id);
          table.ForeignKey(
            name: "fk_materials_groups_group_id",
            column: x => x.group_id,
            principalTable: "groups",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict
          );
          table.ForeignKey(
            name: "fk_materials_lessons_lesson_id",
            column: x => x.lesson_id,
            principalTable: "lessons",
            principalColumn: "id",
            onDelete: ReferentialAction.Cascade
          );
          table.ForeignKey(
            name: "fk_materials_teachers_teacher_id",
            column: x => x.teacher_id,
            principalTable: "teachers",
            principalColumn: "id",
            onDelete: ReferentialAction.Restrict
          );
        }
      );

      migrationBuilder.CreateIndex(
        name: "ix_attendances_lesson_id_student_id",
        table: "attendances",
        columns: new[] { "lesson_id", "student_id" },
        unique: true
      );

      migrationBuilder.CreateIndex(
        name: "ix_attendances_student_id",
        table: "attendances",
        column: "student_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_grades_lesson_id",
        table: "grades",
        column: "lesson_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_grades_student_id",
        table: "grades",
        column: "student_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_group_students_group_id_student_id",
        table: "group_students",
        columns: new[] { "group_id", "student_id" },
        unique: true
      );

      migrationBuilder.CreateIndex(
        name: "ix_group_students_student_id",
        table: "group_students",
        column: "student_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_groups_course_id",
        table: "groups",
        column: "course_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_groups_teacher_id",
        table: "groups",
        column: "teacher_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_lessons_group_id",
        table: "lessons",
        column: "group_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_lessons_teacher_id",
        table: "lessons",
        column: "teacher_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_materials_group_id",
        table: "materials",
        column: "group_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_materials_lesson_id",
        table: "materials",
        column: "lesson_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_materials_teacher_id",
        table: "materials",
        column: "teacher_id"
      );

      migrationBuilder.CreateIndex(
        name: "ix_students_user_id",
        table: "students",
        column: "user_id",
        unique: true
      );

      migrationBuilder.CreateIndex(
        name: "ix_teachers_user_id",
        table: "teachers",
        column: "user_id",
        unique: true
      );

      migrationBuilder.CreateIndex(
        name: "ix_users_email",
        table: "users",
        column: "email",
        unique: true
      );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "attendances");

      migrationBuilder.DropTable(name: "grades");

      migrationBuilder.DropTable(name: "group_students");

      migrationBuilder.DropTable(name: "materials");

      migrationBuilder.DropTable(name: "students");

      migrationBuilder.DropTable(name: "lessons");

      migrationBuilder.DropTable(name: "groups");

      migrationBuilder.DropTable(name: "courses");

      migrationBuilder.DropTable(name: "teachers");

      migrationBuilder.DropTable(name: "users");
    }
  }
}
