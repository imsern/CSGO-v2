# A console app of the game Counter-Strike: Global Offensive.
#### It is all automatic atm, and there is no user inputs controlling the game after it starts.
###### DISCLAIMER: The app is not finished and/or fully functional at this time!


Features:
When the game engine is called from the Program you can set Matchround and Maxrounds. 
Standard is set to be like a competetive match in the original game(max 30 rounds, the one to reach 16 rounds win)


--Choosing sites--
-Both team will choose a site each round. (CT is randomly split between the two sites(A AND B)).
-CT will swap to the same site as T when bomb is planted.

--Weapons/Buymenu--
-Both teams have their own lists of weapons
-Weapons got damage, accuracy and a cost.
-They will check their economy each round, if they can afford weapons they will buy it
 And also check if the entire team can afford before that happens.
-First they buy weapon, then they check if they can afford defusekit and/or armor.

--Fight sequence--
-The game engine runs a Fight method, which calls for a shoot method in both CT and T
-The method for choosing target is also checking so the shooter and the target is at the same site.
-If the target dies from damage, the attacker will get +300 money.
-If all CTs are killed on the site T will plant the bomb. Which will make the CTs on the other site move to their location.
-After the bomb is planted the fight sequence will start again, while a 30sec countdown starts.
-If all Ts dies CT will try and defuse the bomb.(takes 3sec with a defuse kit, 5sec without)

--Win Conditions--
-If either of the team dies, the opposite wins
-If the bomb is planted and the time runs out, Ts win.
-If the bomb is defused on time, CTs win.
-Either of the teams to reach 16 score(or won rounds) will win the match. If the score ends at 15-15 its a draw.

Further plans:

-Add a reload method, and ammo for weapons
-Make it look better and more informative in the console.
