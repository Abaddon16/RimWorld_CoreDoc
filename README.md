# RimWorld_CoreDoc

This is an attempt to document the Core codebase of RimWorld. It's insanity, I know. Just roll with me here. This decompliation is from 2018, so it may be way out of date. I don't even remember how I got the files anymore. Feel free to remind me, should you see something out of date.

## Authority

I emailed Tynan directly and he responded that he believes the purpose of this repo to be a good objective. Any who want proof, I can send the email to you, provided you give me your email address.

I am working on a GitHub pages solution to providing this proof. Additionally, I am working on a .gitignore solution that meets the intent. Once I find it, I will be recreating this repo so it meets the intent.

## Purpose

The intent of this project is to provide a full set of documentation for the RimWorld vanilla code base. Currently, there isn't one. For good reason - this task is daunting.
There is no easy way to do this, it is simply tedious and repetitive work. As one man, I could never finish this to a satisfactory level - there is around... 8000 files? Ballpark.

That was the reason it _hadn't_ been done so far. But what about why I want to do it now? Simple - to enable new modders (like myself) to ease into C# modding without the daunting task of
digging through the source code looking for each function and attribute and trying to figure out what they do and where to find the declaration of each. I jumped in head first with my first attempt
at modding, and it was a bit difficult to say the least. I had no references that were up-to-date (v1.0) and so I was very lost and definitely at the mercy of the modding Discord community.
If not for Mehni and the rest of the community, I wouldn't have even stepped in the door, and would have turned away from this side of the game permanently. I want to open the doors to all those
gamers out there that haven't taken up the mantle of "Modder" because they see C# as daunting - I've seen it time and time again already, and it's time to change that.

## Notes

The current folder structure is simple. One folder, /Codebase. It holds the decompiled source code of the Assembly-CSharp.dll. In these files I have the XML documentation comments.
In the future I look to move on to having an /XML folder to house the constructed XML files. There will also be a /HTML folder in the future when it's completed.

The code in these files probably looks different than you're used to. My VSCode settings are such that all extraneous newlines and spaces are removed.
Newlines after namespace, class, function, method, object, etc. are set to be removed. It will be kept this way, simply because it shortens file lengths by hundreds of lines.
[Thing.cs](/Codebase/Verse/Thing.cs) was reduced by 30% by this alone (before I got Tynan's copy).

I'm not sure how this repo will be used, currently. I'm thinking have it open at the same time, but I'm not sure. If anyone figures it out, let me know. If nothing else, I'll have the comments made, and then some sort of webpage to search through, that people can look at.

## Contribute

To help, follow the steps at [this site](https://akrabat.com/the-beginners-guide-to-contributing-to-a-github-project/). Prior to any help, check the current PRs for the files you want to alter, to reduce any duplicated work.

Summary:

- Fork & clone locally
- Create an upstream remote and sync your local copy before you branch
- Branch for each separate piece of work (try and do small chunks for the sake of simpler PRs)
- Do the work, write good commit messages (list altered files)
- Push to **your** origin (remote/on GitHub) repo
- Create a new PR on GitHub (descriptive PR notes)

### XML Comments

Look at [AddictionUtility.cs](/Codebase/RimWorld/AddictionUtility_Commented.cs) and follow that. It's a merge of what Tynan wrote, and the formatting I've done. It's a huge file, it has plenty of examples. If you run into something you're not sure how to handle, ask.
If I'm not around to answer, take the spirit and obvious intent of the other files I've validated and apply that for the time being. Reengage with me once I'm back.

The intent of these comments is two-fold.

1. Customers can download the repo and use it as an on-computer solution w/ Intellisense hover text for all the functions, properties, method, fields, values, parameters, returns, etc.
2. I can run DOxygen on the folders and create a beautiful webpage manifestation of the documentation we write similar to this site: http://eigen.tuxfamily.org/dox/group__TutorialSparse.html

# Conclusion

So, while it is ambitious, we have a good goal ahead of us. Join me in making the C# side of RimWorld modding just that much easier. - Abaddon16
