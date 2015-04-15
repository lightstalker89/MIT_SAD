using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using NUnit.Framework;
using PassSecure.Data;
using PassSecure.Models;
using PassSecure.Service;

namespace PassSecure.Test
{
    [TestFixture]
    public class PasswordAnalyzerTest
    {
        private readonly Random random = new Random();
        private UserTraining currentUserTraining;
        private PasswordAnalyzer sut;
        private const int PasswordTestDataEntries = 500;
        private const int MaxTimeDiff = 100;
        private const int TimeBetweenUpDown = 10;
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

        [Test]
        public void AnalyzeWithNormalTime()
        {
            List<KeyStroke> keyStrokes = new List<KeyStroke>();
            foreach (Key key in keys)
            {
                int keyDownToAdd = random.Next(0, MaxTimeDiff / 5);
                int keyUpToAdd = random.Next(0, MaxTimeDiff / 5);
                TimeSpan keyDownTime = new TimeSpan(DateTime.Now.Ticks);
                keyDownTime = keyDownTime.Add(new TimeSpan(0, 0, 0, 0, keyDownToAdd));
                TimeSpan keyUpTime = new TimeSpan(DateTime.Now.Ticks);
                keyUpTime = keyUpTime.Add(new TimeSpan(0, 0, 0, 0, TimeBetweenUpDown + keyUpToAdd));
                KeyStroke keyStroke = new KeyStroke(key)
                {
                    KeyDownTime = keyDownTime,
                    KeyUpTime = keyUpTime,
                };
                keyStrokes.Add(keyStroke);
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

        [Test]
        public void AnalyzeWithSlowBeginning()
        {
            List<KeyStroke> keyStrokes = new List<KeyStroke>();
            var count = 0;
            foreach (Key key in keys)
            {
                int keyDownToAdd = 0;
                int keyUpToAdd = 0;
                if (count <= 3)
                {
                    keyDownToAdd = 100;
                    keyUpToAdd = 100;
                }
                TimeSpan keyDownTime = new TimeSpan(DateTime.Now.Ticks);
                keyDownTime = keyDownTime.Add(new TimeSpan(0, 0, 0, 0, keyDownToAdd));
                TimeSpan keyUpTime = new TimeSpan(DateTime.Now.Ticks);
                keyUpTime = keyUpTime.Add(new TimeSpan(0, 0, 0, 0, TimeBetweenUpDown + keyUpToAdd));
                KeyStroke keyStroke = new KeyStroke(key)
                {
                    KeyDownTime = keyDownTime,
                    KeyUpTime = keyUpTime,
                };
                keyStrokes.Add(keyStroke);
                count++;
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

        [Test]
        public void AnalyzeWithSlowEnding()
        {
            List<KeyStroke> keyStrokes = new List<KeyStroke>();
            var count = 0;
            foreach (Key key in keys)
            {
                int keyDownToAdd = 0;
                int keyUpToAdd = 0;
                if (count > 5 &&  count < Password.Length)
                {
                    keyDownToAdd = 50;
                    keyUpToAdd = 50;
                }
                TimeSpan keyDownTime = new TimeSpan(DateTime.Now.Ticks);
                keyDownTime = keyDownTime.Add(new TimeSpan(0, 0, 0, 0, keyDownToAdd));
                TimeSpan keyUpTime = new TimeSpan(DateTime.Now.Ticks);
                keyUpTime = keyUpTime.Add(new TimeSpan(0, 0, 0, 0, TimeBetweenUpDown + keyUpToAdd));
                KeyStroke keyStroke = new KeyStroke(key)
                {
                    KeyDownTime = keyDownTime,
                    KeyUpTime = keyUpTime,
                };
                keyStrokes.Add(keyStroke);
                count++;
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

        [Test]
        public void AnalyzeWithFastBeginning()
        {
          
        }

        [Test]
        public void AnalyzeWithFastEnding()
        {
           
        }

        private void GenerateTestData()
        {
            for (int i = 0; i < PasswordTestDataEntries; i++)
            {
                List<KeyStroke> keyStrokes = new List<KeyStroke>();
                foreach (Key key in keys)
                {
                    int keyDownToAdd = random.Next(0, MaxTimeDiff);
                    int keyUpToAdd = random.Next(0, MaxTimeDiff);
                    TimeSpan keyDownTime = new TimeSpan(DateTime.Now.Ticks);
                    keyDownTime = keyDownTime.Add(new TimeSpan(0, 0, 0, 0, keyDownToAdd));
                    TimeSpan keyUpTime = new TimeSpan(DateTime.Now.Ticks);
                    keyUpTime = keyUpTime.Add(new TimeSpan(0, 0, 0, 0, TimeBetweenUpDown + keyUpToAdd));
                    KeyStroke keyStroke = new KeyStroke(key)
                                          {
                                              KeyDownTime = keyDownTime,
                                              KeyUpTime = keyUpTime,
                                          };
                    keyStrokes.Add(keyStroke);
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
