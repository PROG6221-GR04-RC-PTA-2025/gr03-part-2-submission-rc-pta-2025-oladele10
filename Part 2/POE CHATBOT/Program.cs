using System;
using System.IO;
using System.Collections.Generic;
using NAudio.Wave;
using System.Speech.Synthesis;

namespace poe_part1
{
    class POE_PART
    {
        static SpeechSynthesizer synth = new SpeechSynthesizer();

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
            string? name = Console.ReadLine()?.Trim();

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write("Name cannot be empty. Enter your name: ");
                Console.ForegroundColor = ConsoleColor.White;
                name = Console.ReadLine()?.Trim();
            }

            string welcomeMessage = $"\nHello {name}! Welcome to the SECUNET awareness bot. I'm here to help you stay safe online.";
            Console.WriteLine(welcomeMessage);
            Speak(welcomeMessage);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("\nWould you like me to help you? (Yes/No): ");
            Console.ForegroundColor = ConsoleColor.White;
            string? helpResponse = Console.ReadLine()?.Trim().ToLower();

            if (helpResponse == "no")
            {
                Console.WriteLine("Okay, let me know if you want me to help you later.");
                return;
            }
            else if (helpResponse != "yes")
            {
                Console.WriteLine("Invalid input. Please restart and enter Yes or No.");
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
            // Set the color for the ASCII art
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(@"                   SSSSSS     EEEEEE    CCCCCC    U      U    N      N    EEEEEE     TTTTTTTT");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(@"                  S           E         C         U      U    NN     N    E             T");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@"                  S           E         C         U      U    N N    N    E             T");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"                    SSSSSS    EEEEEE    C         U      U    N  N   N    EEEEEE        T");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(@"                          S   E         C         U      U    N   NNN     E             T");
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(@"                          S   E         C         U      U    N    N      E             T");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine(@"                    SSSSSS    EEEEEE    CCCCCC    UUUUUUU     N      N    EEEEEE        T");

            // Additional message with default color
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("\n                                STAY SAFE | STAY SECURE | STAY CYBERSMART");
        }

        static void StartConversation()
        {
            Dictionary<int, (string topic, string response, string example)> topics = new Dictionary<int, (string, string, string)>
            {
                { 1, ("What are the most common cybersecurity threats?", "The most common threats include phishing, malware, and weak passwords.", "A fake email that looks like it’s from your bank asking for your login details is a phishing attempt.") },
                { 2, ("How can I protect my personal data online?", "You should use strong passwords, enable two-factor authentication, and avoid sharing sensitive information.", "Instead of using '123456' as your password, use a combination of letters, numbers, and symbols.") },
                { 3, ("How can I recognize a secure website?", "Look for 'https://' in the URL, a padlock icon, and avoid sites with security warnings.", "A secure banking site should always use 'https://bankname.com' instead of 'http://'.") },
                { 4, ("Password safety", "Use strong passwords and never share them!", "Instead of using 'password123', use 'Tr0ub4d0r&3'.") },
                { 5, ("Phishing", "Don't click on suspicious links in emails.", "If you get an email from 'PayPal' asking for your password, don't enter it! Always verify the sender.") },
                { 6, ("Hacking risks", "Enable two-factor authentication for extra security.", "Even if a hacker steals your password, 2FA will stop them!") },
                { 7, ("Safe browsing", "Use a VPN and avoid public WiFi for banking.", "Hackers can steal your login details if you use free WiFi at a café.") },
                { 8, ("What should I do if my account is hacked?", "Change your password immediately, enable 2FA, and contact customer support.", "If you can’t access your email, request a password reset from the service provider.") },
                { 9, ("What is malware?", "Malware is malicious software designed to harm or exploit your system.", "A 'free game' you downloaded might secretly steal your passwords.") },
                { 10, ("Exit", "OK GOODBYE! THANKS FOR USING SECUNET STAY SAFE.", "") }
            };

            while (true)
            {
                Console.WriteLine("\n══════════════════════════════════════════════════");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("How would you like me to assist you:");

                foreach (var item in topics)
                {
                    Console.WriteLine($" {item.Key}. {item.Value.topic}");
                }

                Console.WriteLine("══════════════════════════════════════════════════");

                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("\nYour choice: ");
                string? input = Console.ReadLine()?.Trim();
                bool isValidChoice = int.TryParse(input, out int choice) && topics.ContainsKey(choice);

                if (!isValidChoice)
                {
                    Console.WriteLine("Invalid choice. Please enter a valid number from the menu.");
                    continue;
                }

                var selectedTopic = topics[choice];
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine($"\nBot: {selectedTopic.response}");

                if (!string.IsNullOrEmpty(selectedTopic.example))
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine($"Example: {selectedTopic.example}");
                }

                if (choice == 10)
                {
                    Speak(selectedTopic.response);
                    break;
                }
            }
        }
    }
}
