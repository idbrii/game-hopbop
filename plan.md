hopbop
======

2D platformer testbed for validating claims that physics is bad.

After reading eevee's frustratings with unity physics [1] and watching Kyle's talk about deriving the right contants for platforming [2], I wanted to try out making a physics-based platformer.


Results
=======

I found that eevee's claim that wall jumping sticks you to the walls was false for the initial values I used (friction=0.4). Increasing player's friction to 0.75 caused it to stick to walls like she described.

I haven't figured out how to apply Kyle's talk. I think he's assuming you're directly controlling velocity and will set movement numbers on jump.

There's some alternatives that implement physics movement and collision (raycasts):
* SebLague's 2D Platformer tutorial [3]
 * Recommended by comment on Kyle's talk but code style makes me hesitant.
* McQuillan's 2D Platform controller [4]
 * Twitter reply to eevee. Seems okay.


Future Work
===========

Apply Kyle's warped parabolas to make jumps feel nice.



[1]: https://eev.ee/blog/2017/10/13/coaxing-2d-platforming-out-of-unity/
[2]: https://www.youtube.com/watch?v=hG9SzQxaCm8
[3]: https://github.com/SebLague/2DPlatformer-Tutorial/tree/master/Platformer%20E03/Assets/Scripts
[4]: https://pastebin.com/Gdp2tebW
