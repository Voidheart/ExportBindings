using System.IO;
using System.Linq;
using BepInEx;

namespace VoidExportBindings.Core.Patches
{
    public static class ConsolePatch
    {
        public static void Init()
        {
            var fileExt = "keybind";
            new Terminal.ConsoleCommand("exportbinds",
                                        "[filename] Export current key binds to BepInEx/Config folder. No need for file extension",
                                        args =>
                                        {
                                            if (args.Length < 2)
                                            {
                                                args.Context.AddString("Missing arguments [filename]");
                                                return;
                                            }

                                            var filename = args[1];
                                            if (string.IsNullOrEmpty(filename))
                                            {
                                                args.Context.AddString("Filename invalid");
                                                return;
                                            }

                                            var contents = Terminal.m_bindList.Where(x => !string.IsNullOrWhiteSpace(x))
                                                                   .ToArray();
                                            if (contents.Length > 0)
                                            {
                                                var path = Path.Combine(Paths.ConfigPath,
                                                                        Path.GetFileNameWithoutExtension(filename));
                                                path = Path.ChangeExtension(path, fileExt);
                                                File.WriteAllLines(path,
                                                                   contents);
                                            }
                                        });

            new Terminal.ConsoleCommand("importbinds",
                                        "[filename] Import key binds from a file which is located in BepInEx/Config folder. No need for file extension",
                                        args =>
                                        {
                                            if (args.Length < 2)
                                            {
                                                args.Context.AddString("Missing arguments [filename]");
                                                return;
                                            }

                                            var filename = args[1];
                                            if (string.IsNullOrEmpty(filename))
                                            {
                                                args.Context.AddString("Filename invalid");
                                                return;
                                            }

                                            var path = Path.Combine(Paths.ConfigPath,
                                                                    Path.GetFileNameWithoutExtension(filename));
                                            path = Path.ChangeExtension(path, fileExt);
                                            if (!File.Exists(path))
                                            {
                                                args.Context.AddString("Filename invalid");
                                                return;
                                            }


                                            var data = File.ReadAllLines(path).Where(x => !string.IsNullOrWhiteSpace(x))
                                                           .ToArray();
                                            if (data.Length > 0)
                                            {
                                                Terminal.m_bindList.Clear();
                                                // for (var index = 0; index < Terminal.m_bindList.Count; index++)
                                                //     Terminal.m_bindList.Remove(Terminal.m_bindList[index]);

                                                foreach (var bind in data) Terminal.m_bindList.Add(bind);
                                                Terminal.updateBinds();
                                            }
                                            else
                                            {
                                                args.Context.AddString("Malformed binding list");
                                            }
                                        });
        }
    }
}