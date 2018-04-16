using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MeetingSchema.Model;
using MeetingSchema.Model.Entities;

namespace MeetingSchema.Data
{
    public class MeetingSchemaDbInitializer
    {
     
        public static void Initialize(MeetingSchemaContext context)
        {
            InitializeMeetingSchemas(context);
        }

        // Populate the database using Entityframework dotnet command
        private static void InitializeMeetingSchemas(MeetingSchemaContext context)
        {
            if (!context.Users.Any())
            {
                User user_one = new User { Name = "Peter Larsson", Profession = "Designer", Avatar = "avatar_01.png" };
                User user_two = new User { Name = "Hans Gustasson", Profession = "Developer", Avatar = "avatar_02.png" };
                User user_tree = new User { Name = "Lennert Bengsson", Profession = "Backend developer", Avatar = "avatar_03.png" };
                User user_four = new User { Name = "Hellena Bensson", Profession = "Engineer", Avatar = "avatar_04.png" };
                User user_five = new User { Name = "Sara Onofors", Profession = "Fronend developer", Avatar = "avatar_05.png" };
                User user_sex = new User { Name = "Tomas prochery", Profession = "Architecture designer", Avatar = "avatar_02.png" };


                //Add users to DB
                context.Users.Add(user_one);
                context.Users.Add(user_two);
                context.Users.Add(user_tree);
                context.Users.Add(user_four);
                context.Users.Add(user_five);
                context.Users.Add(user_sex);

                //Save EF DB
                context.SaveChanges();
            }

            if (!context.MeetingSchemas.Any())
            {
                MeetingSchemas meetingSchemas_one = new MeetingSchemas
                {
                    Title = "Meeting",
                    Description = "Meeting with developer team",
                    Location = "Word tread center",
                    CreatorId = 1,
                    Status = MeetingSchemaStatus.Valid,
                    Type = MeetingSchemaType.Work,
                    TimeStart = DateTime.Now.AddHours(3),
                    TimeEnd = DateTime.Now.AddHours(6),
                    
                    //Add the participants
                    Participants = new List<Participants>
                    {
                        new Participants() { MeetingSchemaId = 1, UserId = 1},
                        new Participants() { MeetingSchemaId = 1, UserId = 2},
                        new Participants() {MeetingSchemaId = 1 , UserId = 3}
                    }
                };

                //Create instance for coffe  
                MeetingSchemas meetingSchemas_two = new MeetingSchemas
                {
                    Title = "Coffe time",
                    Description = "Coffe time with developer",
                    Location = "Stockholm sale",
                    CreatorId = 2,
                    Status = MeetingSchemaStatus.Valid,
                    Type = MeetingSchemaType.Coffe,
                    TimeStart = DateTime.Now.AddHours(2),
                    TimeEnd = DateTime.Now.AddHours(4),

                    //Add Participants to be at coffe
                    Participants = new List<Participants>
                    {
                        new Participants(){ MeetingSchemaId = 2, UserId= 4 },
                        new Participants() { MeetingSchemaId = 1, UserId = 3},
                        new Participants(){ MeetingSchemaId = 2, UserId = 6}

                    }
                };

                //Create Shoping instance
                MeetingSchemas meetingSchemas_tree = new MeetingSchemas
                {
                    Title = " Shoping plan",
                    Description = "Shoping at Kista galleria",
                    Location = "Kista Galleria",
                    CreatorId = 3,
                    Status = MeetingSchemaStatus.Valid,
                    Type = MeetingSchemaType.Shopping,
                    TimeStart = DateTime.Now.AddHours(4),
                    TimeEnd = DateTime.Now.AddHours(6),
                    
                    //Add participants to shoping timme 
                    Participants = new List<Participants>
                    {
                       new  Participants() { MeetingSchemaId = 3 , UserId = 2 },
                       new Participants(){ MeetingSchemaId = 3, UserId = 4},
                       new Participants(){ MeetingSchemaId = 3, UserId = 6 }
                    }
                };

                //Create instace for famliy meeting 
                MeetingSchemas meetingSchemas_four = new MeetingSchemas
                {
                    Title = "family meeting",
                    Description = "My family will have a meeting at home",
                    Location = "Home",
                    CreatorId = 4,
                    Status = MeetingSchemaStatus.Valid,
                    Type = MeetingSchemaType.Others,
                    TimeStart = DateTime.Now.AddHours(2),
                    TimeEnd = DateTime.Now.AddHours(5),

                    //Add participants
                    Participants = new List<Participants>
                    {
                       new Participants() { MeetingSchemaId = 4 , UserId = 1},
                       new Participants() { MeetingSchemaId = 4, UserId = 3},
                       new Participants() { MeetingSchemaId = 4 , UserId = 4}
                    }
                };

                //Create instace for famliy meeting 
                MeetingSchemas meetingSchemas_five = new MeetingSchemas
                {
                    Title = "Student friends",
                    Description = " Meeting with my friends at KTH",
                    Location = "KTH D-Sale",
                    CreatorId = 5,
                    Status = MeetingSchemaStatus.Valid,
                    Type = MeetingSchemaType.Others,
                    TimeStart = DateTime.Now.AddHours(3),
                    TimeEnd = DateTime.Now.AddHours(5),
                   
                    //Add participants 
                    Participants = new List<Participants>
                    {
                        new Participants() { MeetingSchemaId = 4, UserId = 1 },
                        new Participants() { MeetingSchemaId = 4, UserId = 2 },
                        new Participants() { MeetingSchemaId = 4, UserId = 3 },
                        new Participants() { MeetingSchemaId = 4, UserId = 4 },
                        new Participants() { MeetingSchemaId = 4, UserId = 6}
                    }
                };

                //Create instace for visiting a doctor
                MeetingSchemas meetingSchemas_sex = new MeetingSchemas
                {
                    Title = "Meeting doctor",
                    Description = "Meeting a doctor for employees",
                    Location = "Karolinska hospital",
                    CreatorId = 6,
                    Status = MeetingSchemaStatus.Valid,
                    Type = MeetingSchemaType.Doctor,
                    TimeStart = DateTime.Now.AddHours(2),
                    TimeEnd = DateTime.Now.AddHours(4),
                   
                    //Create instance of participants for doctor meeting
                    Participants = new List<Participants>
                    {
                        new Participants() {MeetingSchemaId = 4, UserId = 1 },
                        new Participants(){ MeetingSchemaId = 4, UserId = 2 },
                        new Participants() { MeetingSchemaId = 4 , UserId = 3 }
                    }
                };

                //Add MeetingSchemas obj to DB
                context.MeetingSchemas.Add(meetingSchemas_one);
                context.MeetingSchemas.Add(meetingSchemas_two);
                context.MeetingSchemas.Add(meetingSchemas_tree);
                context.MeetingSchemas.Add(meetingSchemas_four);
                context.MeetingSchemas.Add(meetingSchemas_five);
                context.MeetingSchemas.Add(meetingSchemas_sex);

                //Save EF DB 
                context.SaveChanges();
            }
        }
    }
}
