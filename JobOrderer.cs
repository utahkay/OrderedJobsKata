using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderedJobsKata
{
    public class JobOrderer
    {
        readonly Dictionary<string, string> jobs = new Dictionary<string, string>();

        public string Output(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            BuildDictionaryOfJobsWithDependencies(input);

            string result = "";
            foreach (var job in jobs.Keys)
            {
                result = DescendTree(job, result, "");
            }
            return result;
        }

        void BuildDictionaryOfJobsWithDependencies(string input)
        {
            var lines = input.Split(new[] {'\n'}, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var tokens = line.Split(new[] {"=>"}, StringSplitOptions.RemoveEmptyEntries);
                var job = tokens[0].Trim();
                var dependency = "";
                if (tokens.Count() > 1)
                    dependency = tokens[1].Trim();

                jobs.Add(job, dependency);
            }
        }

        string DescendTree(string currentJob, string result, string cycleDetect)
        {
            if (JobCompleted(currentJob, result))
                return result;

            if (!HasDependencies(currentJob))
            {
                return result + currentJob;
            }

            if (cycleDetect.Contains(currentJob))
                throw new CycleException();

            cycleDetect += currentJob;
            return DescendTree(jobs[currentJob], result, cycleDetect) + currentJob;
        }

        bool JobCompleted(string currentJob, string result)
        {
            return result.Contains(currentJob);
        }

        bool HasDependencies(string job)
        {
            return !string.IsNullOrEmpty(jobs[job]);
        }
    }
}