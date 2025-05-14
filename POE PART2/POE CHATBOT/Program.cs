using System;
using System.Collections.Generic;
using System.IO;
using System.Speech.Synthesis;
using NAudio.Wave;

namespace poe_part1
{
    class POE_PART
    {
        static SpeechSynthesizer synth = new SpeechSynthesizer();

        static string userName = "";
        static string userInterest = "";

        static readonly HashSet<string> previousQuestions = new HashSet<string>();

        static readonly Dictionary<string, string> sentimentResponses = new Dictionary<string, string>
        {
            { "Worried", "It's completely understandable to feel that way. Let's explore ways to stay safe together." },
            { "Curious", "Curiosity is great! Let's dive into the topic you're interested in." },
            { "Frustrated", "I hear you. Cybersecurity can be overwhelming, but I'm here to guide you step by step." },
            { "Sad", "I'm sorry you're feeling that way. You're not alone—let's focus on something positive and helpful together." },
            { "I'm down", "It’s okay to feel low sometimes. I’m here to support you—let’s take things one step at a time." },
            { "Confused", "No worries! I’ll break it down in a way that’s easy to understand." },
            { "Anxious", "I get that this can feel stressful. Let’s breathe and walk through it together." },
            { "Angry", "It’s okay to feel upset, cyber issues can be really frustrating. Let’s work through the problem calmly." },
            { "Overwhelmed", "Take a deep breath. One step at a time, and I’ll be right here to help you." },
            { "Excited", "That’s awesome! Let’s make the most of that energy and learn something new." },
            { "Tired", "Sounds like you’ve had a long day. Let’s keep it simple and straightforward for now." },
            { "Motivated", "Great to hear! Let’s channel that motivation into learning something useful today." }
        };

        static readonly Dictionary<string, string> keywordResponses = new Dictionary<string, string>
        {
            { "Password", "Use strong, unique passwords. Avoid reusing them across platforms." },
            { "2FA", "Two-Factor Authentication adds an extra layer of security to your accounts." },
            { "Encryption", "Encryption turns data into unreadable code, helping to protect it." },
            { "Firewall", "Firewalls block unauthorized network access. Always keep it active." },
            { "Patching", "Install updates regularly to fix security vulnerabilities in software." },
            { "Malware", "Malware includes viruses and spyware. Keep antivirus software updated." },
            { "Ransomware", "Ransomware locks your files and demands payment. Backup your data." },
            { "Spyware", "Spyware collects data secretly. Avoid downloading from unknown sources." },
            { "vpn", "A VPN hides your IP and encrypts your connection—use it on public Wi-Fi." },
            { "Privacy", "Limit sharing personal info online and review privacy settings often." },
            { "Cyberbullying", "Cyberbullying is digital harassment. Report abuse and protect yourself." },
            { "Phishing", "Don't click suspicious links or give away personal info via email." },
            { "Scam", "Verify sources before responding to suspicious calls, emails, or messages." },
            { "Social engineering", "Scammers may impersonate trusted people. Be cautious before acting." }
        };

        static readonly List<string> phishingTips = new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Always check the sender's email address carefully—it might be a fake.",
            "Never click on suspicious links or download attachments from unknown sources."
        };

        static readonly Dictionary<string, List<string>> topicCategories = new Dictionary<string, List<string>>
        {
            { "account security", new List<string> { "password", "2fa", "encryption" } },
            { "system protection", new List<string> { "firewall", "patching", "malware", "ransomware", "spyware" } },
            { "online safety", new List<string> { "vpn", "privacy", "cyberbullying" } },
            { "threat awareness", new List<string> { "phishing", "scam", "social engineering" } }
        };

        static void Main()
        {
            Console.Title = "SECUNET Chatbot";
            Console.Clear();
            Console.WriteLine("Loading SECUNET Chatbot...");
            PlayVoiceGreeting();
            ShowAsciiArt();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nEnter your name: ");
            Console.ForegroundColor = ConsoleColor.White;
            userName = Console.ReadLine()?.Trim();

            while (string.IsNullOrWhiteSpace(userName))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Name cannot be empty. Enter your name: ");
                Console.ForegroundColor = ConsoleColor.White;
                userName = Console.ReadLine()?.Trim();
            }

            string welcomeMessage = $"\nHello {userName}! Welcome to the SECUNET awareness bot. I'm here to help you stay safe online.";
            Console.WriteLine(welcomeMessage);
            Speak(welcomeMessage);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nWould you like me to help you? (Yes/No): ");
            Console.ForegroundColor = ConsoleColor.White;
            string? helpResponse = Console.ReadLine()?.Trim().ToLower();

            if (helpResponse == "no")
            {
                string goodbyeMessage = "Okay, let me know if you want me to help you later.";
                Console.WriteLine(goodbyeMessage);
                Speak(goodbyeMessage);
                return;
            }
            else if (helpResponse != "yes")
            {
                string errorMessage = "Invalid input. Please restart and enter Yes or No.";
                Console.WriteLine(errorMessage);
                Speak(errorMessage);
                return;
            }

            StartConversation();
        }

        static void PlayVoiceGreeting()
        {
            string audioPath = "welcome.wav";
            if (File.Exists(audioPath))
            {
                using (var audioFile = new AudioFileReader(audioPath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        System.Threading.Thread.Sleep(100);
                    }
                }
            }
            else
            {
                Console.WriteLine("(Audio file missing. Skipping voice greeting.)");
            }
        }

        static void Speak(string text)
        {
            synth.Speak(text);
        }

        static void ShowAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("                   SSSSSS     EEEEEE    CCCCCC    U      U    N      N    EEEEEE     TTTTTTTT");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("                  S           E         C         U      U    NN     N    E             T");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("                  S           E         C         U      U    N N    N    E             T");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("                    SSSSSS    EEEEEE    C         U      U    N  N   N    EEEEEE        T");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("                          S   E         C         U      U    N   NNN     E             T");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("                          S   E         C         U      U    N    N      E             T");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("                    SSSSSS    EEEEEE    CCCCCC    UUUUUUU     N      N    EEEEEE        T");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n                                STAY SAFE | STAY SECURE | STAY CYBERSMART");
        }

        static void StartConversation()
        {
            while (true)
            {
                Console.WriteLine("\n══════════════════════════════════════════════════");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("Choose a topic category or enter a keyword directly:");
                Console.WriteLine("1. Account Security");
                Console.WriteLine("2. System Protection");
                Console.WriteLine("3. Online Safety");
                Console.WriteLine("4. Threat Awareness");
                Console.WriteLine("Type the number or enter a keyword (e.g., 'password', 'vpn', 'phishing'). Type 'exit' to quit.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nYou: ");
                string? input = Console.ReadLine()?.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    string noUnderstanding = "I'm not sure I understand. Can you try rephrasing?";
                    Console.WriteLine(noUnderstanding);
                    Speak(noUnderstanding);
                    continue;
                }

                if (input == "exit")
                {
                    string goodbyeMessage = "Goodbye! Stay safe online.";
                    Console.WriteLine(goodbyeMessage);
                    Speak(goodbyeMessage);
                    break;
                }

                if (previousQuestions.Contains(input))
                {
                    string repeatMessage = "You've already asked this question, but I'll answer again:";
                    Console.WriteLine(repeatMessage);
                    Speak(repeatMessage);
                }
                else
                {
                    previousQuestions.Add(input);
                }

                if (input == "1") input = "account security";
                else if (input == "2") input = "system protection";
                else if (input == "3") input = "online safety";
                else if (input == "4") input = "threat awareness";

                if (topicCategories.ContainsKey(input))
                {
                    string categoryMessage = $"\nTopics under {input.ToUpper()}:";
                    Console.WriteLine(categoryMessage);
                    Speak(categoryMessage);

                    foreach (string topic in topicCategories[input])
                    {
                        string topicMessage = $"- {topic}";
                        Console.WriteLine(topicMessage);
                        Speak(topicMessage);
                    }

                    Console.WriteLine("\nEnter one of the topics above to learn more:");
                    continue;
                }

                bool sentimentFound = false;
                foreach (var sentiment in sentimentResponses)
                {
                    if (input.Contains(sentiment.Key.ToLower()))
                    {
                        Console.WriteLine($"\n{sentiment.Value}");
                        Speak(sentiment.Value);
                        sentimentFound = true;
                        break;
                    }
                }

                if (sentimentFound) continue;

                bool keywordFound = false;
                foreach (var keyword in keywordResponses)
                {
                    if (input.Contains(keyword.Key.ToLower()))
                    {
                        userInterest = keyword.Key;
                        Console.WriteLine($"\n{keyword.Value}");
                        Speak(keyword.Value);
                        keywordFound = true;
                        break;
                    }
                }

                if (input.Contains("phishing"))
                {
                    Random rnd = new Random();
                    string tip = phishingTips[rnd.Next(phishingTips.Count)];
                    Console.WriteLine($"\n{tip}");
                    Speak(tip);
                    continue;
                }

                if (!sentimentFound && !keywordFound)
                {
                    string unsureMessage = "I'm not sure I understand. Can you try rephrasing?";
                    Console.WriteLine(unsureMessage);
                    Speak(unsureMessage);
                }
                else if (!string.IsNullOrWhiteSpace(userInterest))
                {
                    string interestMessage = $"As someone interested in {userInterest}, remember to keep exploring ways to stay secure.";
                    Console.WriteLine(interestMessage);
                    Speak(interestMessage);
                }
            }
        }
    }
}
