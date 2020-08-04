namespace RunningDinner.Models.DatabaseModels
{
    public class Route
    {
        public int Id { get; set; }
        public DinnerEvent Event { get; set; }

        public Team RouteForTeam { get; set; }

        public Team FirstCourseHostTeam { get; set; }

        public Team FirstCourseGuestTeam1 { get; set; }

        public Team FirstCourseGuestTeam2 { get; set; }

        public Team SecondCourseHostTeam { get; set; }

        public Team SecondCourseGuestTeam1 { get; set; }

        public Team SecondCourseGuestTeam2 { get; set; }

        public Team ThirdCourseHostTeam { get; set; }

        public Team ThirdCourseGuestTeam1 { get; set; }

        public Team ThirdCourseGuestTeam2 { get; set; }

    }
}
