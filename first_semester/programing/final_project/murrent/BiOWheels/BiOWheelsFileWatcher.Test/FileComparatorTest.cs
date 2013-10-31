using System.Collections.Generic;
using System.IO;
using NUnit.Framework;

namespace BiOWheelsFileWatcher.Test
{
    public class FileComparatorTest
    {
        private const string FileNameSourceFile = "A.txt";

        private const string FileNameDestinationFile = "B.txt";

        private const string FileNameSourceFileSame = "AI.txt";

        private const string FileNameDestinationFileSame = "BII.txt";

        /// <summary>
        /// 
        /// </summary>
        private readonly List<string> randomText = new List<string>();

        /// <summary>
        /// 
        /// </summary>
        private FileComparator fileComparator;

        /// <summary>
        /// Set up test environment
        /// </summary>
        [SetUp]
        public void Init()
        {
            this.fileComparator = new FileComparator();
            this.fileComparator.BlockSize = 4096;

            this.randomText.Add("Literature admiration frequently indulgence announcing are who you her. Was least quick after six. So it yourself repeated together cheerful. Neither it cordial so painful picture studied if. Sex him position doubtful resolved boy expenses. Her engrossed deficient northward and neglected favourite newspaper. But use peculiar produced concerns ten. In friendship diminution instrument so. Son sure paid door with say them. Two among sir sorry men court. Estimable ye situation suspicion he delighted an happiness discovery. Fact are size cold why had part. If believing or sweetness otherwise in we forfeited. Tolerably an unwilling arranging of determine. Beyond rather sooner so if up wishes or. Preserved defective offending he daughters on or. Rejoiced prospect yet material servants out answered men admitted. Sportsmen certainty prevailed suspected am as. Add stairs admire all answer the nearer yet length. Advantages prosperous remarkably my inhabiting so reasonably be if. Too any appearance announcing impossible one. Out mrs means heart ham tears shall power every. Be me shall purse my ought times. Joy years doors all would again rooms these. Solicitude announcing as to sufficient my. No my reached suppose proceed pressed perhaps he. Eagerness it delighted pronounce repulsive furniture no. Excuse few the remain highly feebly add people manner say. It high at my mind by roof. No wonder worthy in dinner. ");
            this.randomText.Add("To they four in love. Settling you has separate supplied bed. Concluded resembled suspected his resources curiosity joy. Led all cottage met enabled attempt through talking delight. Dare he feet my tell busy. Considered imprudence of he friendship boisterous. ");
            this.randomText.Add("Do commanded an shameless we disposing do. Indulgence ten remarkably nor are impression out. Power is lived means oh every in we quiet. Remainder provision an in intention. Saw supported too joy promotion engrossed propriety. Me till like it sure no sons.");

        }

        /// <summary>
        /// 
        /// </summary>
        [TestCase]
        public void TestFileComparatorWithDifferentFiles()
        {
            this.CreateFilesWithDifferentContentIfNotExist();

            bool result = this.fileComparator.Compare(FileNameSourceFile, FileNameDestinationFile, 0);

            Assert.NotNull(result);
            Assert.IsFalse(result);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestCase]
        public void TestFileComparatorWithSameFiles()
        {
            this.CreateFilesWithSameContentIfNotExist();

            bool result = this.fileComparator.Compare(FileNameSourceFileSame, FileNameDestinationFileSame, 0);

            Assert.NotNull(result);
            Assert.IsTrue(result);
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateFilesWithDifferentContentIfNotExist()
        {
            if (!File.Exists(FileNameSourceFile))
            {
                using (StreamWriter streamWriter = new StreamWriter(FileNameSourceFile, false))
                {
                    streamWriter.Write(this.randomText[0]);
                }
            }

            if (!File.Exists(FileNameDestinationFile))
            {
                using (StreamWriter streamWriter = new StreamWriter(FileNameDestinationFile, false))
                {
                    streamWriter.Write(string.Empty);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateFilesWithSameContentIfNotExist()
        {
            if (!File.Exists(FileNameSourceFileSame))
            {
                using (StreamWriter streamWriter = new StreamWriter(FileNameSourceFileSame, false))
                {
                    streamWriter.Write(this.randomText[2]);
                }
            }

            if (!File.Exists(FileNameDestinationFileSame))
            {
                using (StreamWriter streamWriter = new StreamWriter(FileNameDestinationFileSame, false))
                {
                    streamWriter.Write(this.randomText[2]);
                }
            }
        }
    }
}
