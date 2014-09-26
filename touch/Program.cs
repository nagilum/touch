using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace touch {
	class Program {
		static void Main(string[] args) {
			if (!args.Any() ||
			    args.SingleOrDefault(a => a == "--help") != null ||
				args.SingleOrDefault(a => a == "/?") != null) {
				writeHelpScreen();
				return;
			}

			var filesToModify = new List<string>();
			var createIfNonExistant = true;

			var setLastAccessTime = false;
			var setLastWriteTime = false;
			var lastAccessTime = DateTime.Now;
			var lastWriteTime = DateTime.Now;

			var error = string.Empty;

			for (var i = 0; i < args.Length; i++) {
				switch (args[i]) {
					case "-a":
						setLastWriteTime = false;
						break;

					case "-m":
						setLastAccessTime = false;
						break;
						
					case "-am":
						setLastAccessTime = true;
						setLastWriteTime = true;
						break;

					case "-c":
						createIfNonExistant = false;
						break;

					case "-d":
						if (i < (args.Length - 1)) {
							DateTime temp;

							if (DateTime.TryParse(args[i + 1], out temp)) {
								lastAccessTime = temp;
								lastWriteTime = temp;

								setLastAccessTime = true;
								setLastWriteTime = true;

								i++;
							}
							else {
								error = "Could not parse the given date-time string: \"" + args[i + 1] + "\"";
							}
						}
						else {
							error = "-d must be followed by a date-time string which will be parsed.";
						}
						break;

					case "-r":
						if (i < (args.Length - 1)) {
							try {
								var fileInfo = new FileInfo(args[i + 1]);

								lastAccessTime = fileInfo.LastAccessTime;
								lastWriteTime = fileInfo.LastWriteTime;

								setLastAccessTime = true;
								setLastWriteTime = true;

								i++;
							}
							catch (Exception ex) {
								error = ex.Message;
							}
						}
						else {
							error = "-r must be followed by a filename to read date-time from.";
						}
						break;

					case "-b":
					case "-B":
						if (i < (args.Length - 1)) {
							var seconds = 0;

							if (int.TryParse(args[i + 1], out seconds)) {
								lastAccessTime = lastWriteTime.AddSeconds(seconds);
								lastWriteTime = lastWriteTime.AddSeconds(seconds);

								setLastAccessTime = true;
								setLastWriteTime = true;

								i++;
							}
							else {
								error = "Could not parse the given number of seconds: \"" + args[i + 1] + "\"";
							}
						}
						else {
							error = "-b must be followed by a number of seconds to add.";
						}
						break;

					default:
						filesToModify.Add(args[i]);
						break;
				}

				if (!string.IsNullOrWhiteSpace(error)) {
					break;
				}
			}

			if (string.IsNullOrWhiteSpace(error) &&
			    filesToModify.Count == 0) {
				error = "No files have been specified for modification/creation.";
			}

			if (!string.IsNullOrWhiteSpace(error)) {
				Console.WriteLine("Error");
				Console.WriteLine(" " + error);
				Console.WriteLine("");

				writeHelpScreen();
				return;
			}

			if (createIfNonExistant) {
				foreach (var filename in filesToModify.Where(filename => !File.Exists(filename))) {
					try {
						File.WriteAllText(filename, null);
					}
					catch (Exception ex) {
						Console.WriteLine("Error");
						Console.WriteLine(" Could not create file: \"" + filename + "\"");
						Console.WriteLine(" " + ex.Message);
						Console.WriteLine("");
					}
				}
			}

			if (!setLastAccessTime &&
				!setLastWriteTime)
				return;

			foreach (var filename in filesToModify.Where(File.Exists)) {
				if (setLastAccessTime) {
					try {
						File.SetLastAccessTime(filename, lastAccessTime);
					}
					catch (Exception ex) {
						Console.WriteLine("Error");
						Console.WriteLine(" Could not set last-access-time on file: \"" + filename + "\"");
						Console.WriteLine(" " + ex.Message);
						Console.WriteLine("");
					}
				}
				if (setLastWriteTime) {
					try {
						File.SetLastWriteTime(filename, lastWriteTime);
					}
					catch (Exception ex) {
						Console.WriteLine("Error");
						Console.WriteLine(" Could not set last-write-time on file: \"" + filename + "\"");
						Console.WriteLine(" " + ex.Message);
						Console.WriteLine("");
					}
				}
			}
		}

		static void writeHelpScreen() {
			Console.WriteLine("Syntax");
			Console.WriteLine(" touch [options] file(s)");
			Console.WriteLine("");

			Console.WriteLine("Description");
			Console.WriteLine(" The touch command updates the access and modification times of each file to");
			Console.WriteLine(" the current system time.");
			Console.WriteLine("");
			Console.WriteLine(" If you specify files that does not already exist, touch creates empty files");
			Console.WriteLine(" with those names (unless the -c option is specified; see below).");
			Console.WriteLine("");

			Console.WriteLine("Options");
			Console.WriteLine(" -a          Change only the access time.");
			Console.WriteLine(" -c          Do not create any files.");
			Console.WriteLine(" -d string   Parse the string and use it instead of the current time.");
			Console.WriteLine(" -m          Change only the modification time.");
			Console.WriteLine(" -r file     Use the file's time instead of current time.");
			Console.WriteLine(" -b seconds  Add number of seconds to the referenced time.");
			Console.WriteLine("");

			Console.WriteLine("Example");
			Console.WriteLine(" touch file.txt");
		}
	}
}
