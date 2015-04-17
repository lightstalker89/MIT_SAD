using System;
using System.Collections.Generic;
using System.Windows.Input;
using NUnit.Framework;
using PassSecure.Data;
using PassSecure.Models;
using PassSecure.Service;

namespace PassSecure.Test
{
    public class PasswordAnalyzerTest
    {
        private UserTraining currentUserTraining;
        private PasswordAnalyzer sut;
        private const int PasswordTestDataEntries = 500;
        private const int MaxTimeDiff = 220;
        private const int TimeBetweenUpDown = 110;
        private readonly Key[] keys = { Key.M, Key.A, Key.R, Key.I, Key.O, Key.M, Key.U, Key.R, Key.R, Key.E, Key.N, Key.T };
        private const string Password = "mariomurrent";
        private const string Username = "mariomurrent";

        /// <summary>
        /// 
        /// </summary>
        /// 
        [TestFixtureSetUp]
        protected void SetUp()
        {
            SimpleContainer.Register(new DataManager());
            SimpleContainer.Register(new DataStore());
            SimpleContainer.Resolve<DataStore>().AddUserTraining(
                   new UserTraining()
                   {
                       UserName = Username,
                       Password = Password
                   });
            sut = new PasswordAnalyzer();
            currentUserTraining = SimpleContainer.Resolve<DataStore>().GetUserTraining(Username);
            GenerateTestData();
        }

        /// <summary>
        /// Test the password analyzer with a password entry with no offset
        /// </summary>
        [Test]
        public void AnalyzeWithoutOffset()
        {
            List<KeyStroke> keyStrokes = new List<KeyStroke>();
            int counter = 1;
            foreach (Key key in keys)
            {
                TimeSpan keyDownTime = new TimeSpan(0, 0, 0, 0, counter * TimeBetweenUpDown);
                TimeSpan keyUpTime = new TimeSpan(0, 0, 0, 0, counter * TimeBetweenUpDown + TimeBetweenUpDown);
                KeyStroke keyStroke = new KeyStroke(key)
                {
                    KeyDownTime = keyDownTime,
                    KeyUpTime = keyUpTime,
                };
                keyStrokes.Add(keyStroke);
                counter++;
            }
            UserTraining passwordEntry = new UserTraining()
            {
                Password = currentUserTraining.Password,
                UserName = currentUserTraining.UserName,
                Trainings = new List<TrainingEntry>()
                                                                      {
                                                                          new TrainingEntry(){ KeyStrokes = keyStrokes}
                                                                      }
            };
            passwordEntry.Analyze();
            Enums.PasswordStatus status = sut.IsAccepted(Username, passwordEntry);
            Assert.IsTrue(status == Enums.PasswordStatus.Accepted);
        }

        /// <summary>
        /// Test the analyzer with a password entry where one keyup time takes longer than normal
        /// </summary>
        [Test]
        public void AnalyzeWithFastBeginning()
        {
            List<KeyStroke> keyStrokes = new List<KeyStroke>();
            int counter = 1;
            foreach (Key key in keys)
            {
                TimeSpan keyDownTime = new TimeSpan(0, 0, 0, 0, counter * TimeBetweenUpDown);
                TimeSpan keyUpTime = new TimeSpan(0, 0, 0, 0, counter * TimeBetweenUpDown + TimeBetweenUpDown);
                if (counter == 2)
                {
                    keyUpTime = keyUpTime.Add(new TimeSpan(0, 0, 0, 0, 500));
                }
                if (counter == Password.Length - 1)
                {
                    keyDownTime = keyDownTime.Subtract(new TimeSpan(0, 0, 0, 0, 500));
                    keyUpTime = keyUpTime.Subtract(new TimeSpan(0, 0, 0, 0, 500));
                }
                KeyStroke keyStroke = new KeyStroke(key)
                {
                    KeyDownTime = keyDownTime,
                    KeyUpTime = keyUpTime,
                };
                keyStrokes.Add(keyStroke);
                counter++;
            }
            UserTraining passwordEntry = new UserTraining()
            {
                Password = currentUserTraining.Password,
                UserName = currentUserTraining.UserName,
                Trainings = new List<TrainingEntry>()
                                                                      {
                                                                          new TrainingEntry(){ KeyStrokes = keyStrokes}
                                                                      }
            };
            passwordEntry.Analyze();
            Enums.PasswordStatus status = sut.IsAccepted(Username, passwordEntry);
            Assert.IsTrue(status == Enums.PasswordStatus.NotAccepted);
        }

        private void GenerateTestData()
        {
         for (int i = 0; i < PasswordTestDataEntries; i++)
            {
                List<KeyStroke> keyStrokes = new List<KeyStroke>();
                int counter = 1;
                foreach (Key key in keys)
                {
                    TimeSpan keyDownTime = new TimeSpan(0,0,0,0, counter * TimeBetweenUpDown);
                    TimeSpan keyUpTime = new TimeSpan(0,0,0,0, counter * TimeBetweenUpDown + TimeBetweenUpDown);
                    KeyStroke keyStroke = new KeyStroke(key)
                    {
                        KeyDownTime = keyDownTime,
                        KeyUpTime = keyUpTime,
                    };
                    keyStrokes.Add(keyStroke);
                    counter++;
                }
                TrainingEntry trainingEntry = new TrainingEntry() { KeyStrokes = keyStrokes, PasswordLength = currentUserTraining.Password.Length };
                trainingEntry.Analyze();
                trainingEntry.Distance = sut.CalculateDistance(currentUserTraining, trainingEntry);
                currentUserTraining.Trainings.Add(trainingEntry);
                currentUserTraining.AcceptedUserAttempt = false;
                currentUserTraining.Analyze();
            }
            SimpleContainer.Resolve<DataStore>().UpdateUserTraining();
        }
    }
}
