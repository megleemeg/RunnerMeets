using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RunnerMeets.Migrations
{
    /// <inheritdoc />
    public partial class AddRunningEventModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "places",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    google_place_id = table.Column<long>(type: "bigint", nullable: true),
                    name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    formatted_address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    place_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_places", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "runner_profiles",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_profile_id = table.Column<long>(type: "bigint", nullable: false),
                    bio = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    experience_level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    min_pace_seconds_per_km = table.Column<int>(type: "integer", nullable: true),
                    max_pace_seconds_per_km = table.Column<int>(type: "integer", nullable: true),
                    training_for = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    preferred_distance_km = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_runner_profiles", x => x.id);
                    table.ForeignKey(
                        name: "fk_runner_profiles_user_profiles_user_profile_id",
                        column: x => x.user_profile_id,
                        principalTable: "user_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "events",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    place_id = table.Column<long>(type: "bigint", nullable: false),
                    created_by_user_profile_id = table.Column<long>(type: "bigint", nullable: false),
                    start_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    route_polyline = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    max_participants = table.Column<int>(type: "integer", nullable: false),
                    min_participants = table.Column<int>(type: "integer", nullable: false),
                    min_pace_seconds_per_km = table.Column<int>(type: "integer", nullable: false),
                    max_pace_seconds_per_km = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    visibility = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    location = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    distance_km = table.Column<decimal>(type: "numeric(5,2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_events", x => x.id);
                    table.ForeignKey(
                        name: "fk_events_places_place_id",
                        column: x => x.place_id,
                        principalTable: "places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_events_user_profiles_created_by_user_profile_id",
                        column: x => x.created_by_user_profile_id,
                        principalTable: "user_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "supply_stations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    place_id = table.Column<long>(type: "bigint", nullable: false),
                    station_type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_supply_stations", x => x.id);
                    table.ForeignKey(
                        name: "fk_supply_stations_places_place_id",
                        column: x => x.place_id,
                        principalTable: "places",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "event_time_options",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    event_id = table.Column<long>(type: "bigint", nullable: false),
                    start_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_time_options", x => x.id);
                    table.ForeignKey(
                        name: "fk_event_time_options_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_time_options_user_profiles_created_by_id",
                        column: x => x.created_by_id,
                        principalTable: "user_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_to_hosts",
                columns: table => new
                {
                    event_id = table.Column<long>(type: "bigint", nullable: false),
                    user_profile_id = table.Column<long>(type: "bigint", nullable: false),
                    role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_to_hosts", x => new { x.event_id, x.user_profile_id });
                    table.ForeignKey(
                        name: "fk_event_to_hosts_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_to_hosts_user_profiles_user_profile_id",
                        column: x => x.user_profile_id,
                        principalTable: "user_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_to_registrations",
                columns: table => new
                {
                    event_id = table.Column<long>(type: "bigint", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    registration_status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    joined_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()"),
                    cancelled_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_to_registrations", x => new { x.event_id, x.user_id });
                    table.ForeignKey(
                        name: "fk_event_to_registrations_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_to_registrations_user_profiles_user_id",
                        column: x => x.user_id,
                        principalTable: "user_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_to_supply_stations",
                columns: table => new
                {
                    event_id = table.Column<long>(type: "bigint", nullable: false),
                    supply_station_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_to_supply_stations", x => new { x.event_id, x.supply_station_id });
                    table.ForeignKey(
                        name: "fk_event_to_supply_stations_events_event_id",
                        column: x => x.event_id,
                        principalTable: "events",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_to_supply_stations_supply_stations_supply_station_id",
                        column: x => x.supply_station_id,
                        principalTable: "supply_stations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "event_time_votes",
                columns: table => new
                {
                    event_time_option_id = table.Column<long>(type: "bigint", nullable: false),
                    user_profile_id = table.Column<long>(type: "bigint", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_event_time_votes", x => new { x.event_time_option_id, x.user_profile_id });
                    table.ForeignKey(
                        name: "fk_event_time_votes_event_time_options_event_time_option_id",
                        column: x => x.event_time_option_id,
                        principalTable: "event_time_options",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_event_time_votes_user_profiles_user_profile_id",
                        column: x => x.user_profile_id,
                        principalTable: "user_profiles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_event_time_options_created_by_id",
                table: "event_time_options",
                column: "created_by_id");

            migrationBuilder.CreateIndex(
                name: "ix_event_time_options_event_id",
                table: "event_time_options",
                column: "event_id");

            migrationBuilder.CreateIndex(
                name: "ix_event_time_options_start_time",
                table: "event_time_options",
                column: "start_time");

            migrationBuilder.CreateIndex(
                name: "ix_event_time_votes_user_profile_id",
                table: "event_time_votes",
                column: "user_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_event_to_hosts_user_profile_id",
                table: "event_to_hosts",
                column: "user_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_event_to_registrations_registration_status",
                table: "event_to_registrations",
                column: "registration_status");

            migrationBuilder.CreateIndex(
                name: "ix_event_to_registrations_user_id",
                table: "event_to_registrations",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_event_to_supply_stations_supply_station_id",
                table: "event_to_supply_stations",
                column: "supply_station_id");

            migrationBuilder.CreateIndex(
                name: "ix_events_created_by_user_profile_id",
                table: "events",
                column: "created_by_user_profile_id");

            migrationBuilder.CreateIndex(
                name: "ix_events_place_id",
                table: "events",
                column: "place_id");

            migrationBuilder.CreateIndex(
                name: "ix_events_start_date_time",
                table: "events",
                column: "start_date_time");

            migrationBuilder.CreateIndex(
                name: "ix_events_status",
                table: "events",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_events_visibility",
                table: "events",
                column: "visibility");

            migrationBuilder.CreateIndex(
                name: "ix_places_google_place_id",
                table: "places",
                column: "google_place_id");

            migrationBuilder.CreateIndex(
                name: "ix_runner_profiles_user_profile_id",
                table: "runner_profiles",
                column: "user_profile_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_supply_stations_place_id",
                table: "supply_stations",
                column: "place_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "event_time_votes");

            migrationBuilder.DropTable(
                name: "event_to_hosts");

            migrationBuilder.DropTable(
                name: "event_to_registrations");

            migrationBuilder.DropTable(
                name: "event_to_supply_stations");

            migrationBuilder.DropTable(
                name: "runner_profiles");

            migrationBuilder.DropTable(
                name: "event_time_options");

            migrationBuilder.DropTable(
                name: "supply_stations");

            migrationBuilder.DropTable(
                name: "events");

            migrationBuilder.DropTable(
                name: "places");
        }
    }
}
