# Megalograph

Hello gametypers!

this is a basic (xml plugin driven) tool for deciphering the contents of the magical .bin files

Focused on H2A gametypes, this tool can decompile the (or rather, some) gamemodes into xml based files that can be interpretted (by this tool) into some gross from of node graph

you get some basic node creation/altering tools, but as of yet, the tools are quite bare-bones because i haven't finished this project yet

this tool uses dot net 6.0, which you will probably need to download to beable to run the tool (https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-6.0.201-windows-x64-installer)

old video of me making a gamemode with it (this was v0.1, so some stuff is out-dated, new one coming tomorrow probably)
https://youtu.be/2WY5gBvY2lI?t=52

ok so i haven't entirely tested this on reach (because there are much better tools out there) so dont expect this tool to beable to open halo reach gamemodes, except if they were like one of the 7 default custom-game gamemodes

also dont entirely expect it to work for halo 4 (because again there are better tools out there)

mainly this was designed for H2A gamemodes

EDIT: i would like to warn RVT users that this tool does not do everything for you like RVT does, you have to figure a lot of it out yourself

# Usage?

to get started, find a .bin file from one of the supported halos (halo reach, halo 4 or halo 2A)

you could also look for the bin files provided by the tool (like the blank 2A one)

open that with megalograph (with the "open .bin" button on the main toolbar)

once you've got that, hit decompile. (it may take a while, im not the best programmer lol)

if you got no errors then it worked, and you can mess with the gametype scripts, in the node window


# Controls

node window controls are: 

- press&hold middle mouse to navigate
- scroll wheel to zoom/unzoom
- ctrl scroll wheel to vertically scroll
- shift scroll wheel to horizontally scroll
- left click to drag / drag select
- right mouse to pop up the basic node creation window
- drag at the little node connection boxes to draw connections
- ctrl A to select all nodes
- shift select nodes for multiselect
- ctrl select to just move comment blocks, and not blocks contained
- ctrl F to search for text inside of nodes

# Metaviewer

and then theres the meta viewer as i like to call it

basically it structures the decompiled megalograph gametype xmls

there are a few handy tools in there, such as the count item creation/removal and string table compilers (which you can thank kornman for cause it was his code that i borrowed for string table stuff)

(if you want to remove specific ones, it may be easier to edit the xml)



# Notes

copy & pasting doesn't remember which nodes were connected (yet)

it does not save nodes that are not connected to anything

nodes cant read some dynamic gamemode things (eg: global objects) as they are clumped together with other options in the enums, would have to make a hybrid enum&reference value type




# Todo list

TODO (v1.0)
- range values
- database max attribute
- database default attribute
- generate blank gamemode from defaults
- negative number support
- warning logger - add warnings for things that can cause the game to not like the gametype
- better exception handling (i think we removed exception handling in v0.2)
- fix meta blocks to look good
- duplication fix - make use of that cool system so it can do the lines & locations when pasting
- rewrite database to remove "Var" from everything
- FIX PROBABLY BROKEN CONSOLE
- better database rearrangements
- prevent copy pasting branch into reach (it crashes if you were wondering)
- auto connect functionality
- insertion point indexes are not updated when triggers indexes are
- update export funcationality to support .numbers & stuff
- record non-connected nodes
- falisafe feedback - output things that failed to be read correctly
- max exceeded error checking
- load sgf xml from source rather than mem
- REWRITE NODES - THE BIG REWRITE (post decomp touchup the scripts into human readable code)
- user defined code blocks aka functions/macros
- make a popup telling that compile was successfull
- vehicle groups not mapped out
- fix if count is open when adding new count item
- middle click dont release while outside of nodegraph
- same with other imputs probably
- count item does not infact save when focus lost due to deletion

TODO (v2.0)
- text code editor
- those line things to connect variables with saving?

UI work
- toolbar icons



# Credits 

i would like to give a thanks to everyone whos helped me (and the gametype community) reach this point
(i cant actually remember everyone because its been such a long journey haha)

first shoutouts would go to Krevil & Arttumiro, whom i looked up to & for providing the helping hands that i needed, to learn the arts of modding halo

shoutouts to the OGs: kornman & DavidJCobb, for all their hard work on their respective tools, which paved the way for gametype modding to even be as big as it is right now

shoutout to ICantFunkUp & LanHikari, the first of many gametyper friends i would make

shoutout to the crew that formed HaloPogSwitch: Killswitch, Cozi, Cantaloupe & Slen, this project helped so much with learning software&reverse engineering, enabling me to even beable to make this tool

my appreciation to the big halo modders who basically formed the entire halo modding community: Gamecheat & Lord Zedd

brooen & greenknight for working on the mammoth in halo reach mod with me (mammoths are cool)

thank you to the two most devoted playtesters: Clearcardinal & HaloNMincrft (and slen but i already put you down for something else), 
i guess i should mention The Ark guys too, becasue i forced them all to endure my earliest cursed gamemodes

Big thankyou to whoronavirus for being the massive legend that you are, and doing your absolute best to bring awareness to the gametyping community
and to BigStack & Adderson for allowing us to host our content on your site

thanks to sofasleeper, trustysnooze & rushmypancake for keeping the scene alive with FIRE gametypes

and thanks to Waffle for his work on the mjolnir forge editor, which has assisted many many forgers (including myself) in creating maps & conducting research

cheers to the newer guys in the scene (or at least i think they're newer) rabidmagicman & weesee I, 
for keeping me on my feet with all these questions, constantly making me doubt myself. and well, for being a part of the community, too

shoutout to the peeps that popped up outta nowhere to shape the Infinite modding scene with me: xxZxx, Baxvius, bissuetox, Callum Carmicheal & Sopitive

lastly, big thankyou to General Trex, for always keeping me on track. i probably would have given up on this project cause no one else really cared lol

last lastly, a thankyou to all the rest of the gametypers who i've met along the way: Bagof, Charlieseeese, drapperbat, firerainV, Kruse002, Ausjacmacian & JonnyOThan

its been an honour to meet all of you guys, and to be provided the opportunity to shape the future of gametyping (even if this tool does go by majorly unused)
love you all
