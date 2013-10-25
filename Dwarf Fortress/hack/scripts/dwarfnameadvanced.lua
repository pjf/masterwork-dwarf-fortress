--Nameparts by http://jtevans.kilnar.com/rpg/dnd/tools/dwarf.php, Aerval, and Andux
local firstnamepart = {
	"ad", "ak", "an", "ar", "ash", "at",
	"ba", "bal", "bar", "bel", "ber", "bil", "bof", "bol", "bro", "bul",
	"cal", "chal",
	"da", "das", "dal", "dim", "dor", "dru", "dur", "dwe", "dwin",
	"el",
	"fal", "far", "fash", "fel", "fim", "ful", "fun",
	"gal", "gar", "ger", "gil", "gim", "gom", "gol", "gri", "gro", "gru", "grun",
	"ha", "hal", "ham", "har", "hel", "her", "ho", "hol", "hor", "hul",
	"ing", "in", "ir",

	"kal", "kas", "ket", "kha", "khor", "kil", "kin", "ko", "kor", "kul",
	"lam", "lar", "lon", "lun",
	"mag", "mal", "mar", "mor", "mun",
	"nal", "nar", "nil", "nor",
	"ol", "or", "ov",

	"rag", "ral", "ram", "rim", "ron", "run", 
	"sal", "sar", "shal", "shar", "shor", "sim", "sor", "stal", "stav", "sven",
	"tan", "tha", "thal", "thin", "thor", "thra", "thu", 
	"um", "ur",
	"val", "van",
	"wil",

	"yar",
	"za", "zir", "zul"
};
local female_secondpart = {
	"a", "ada", "ala", "ana", "ani", "atha",
	"bari", "bina", "bine", "bura", 
	"cha", "chi",
	"da", "dana", "dani", "dili", "dina", "dola", "dora", "dri", "dria", 
	"edi", "ena", "eta", "eva", "eza",
	"fani", "fi",
	"ga", "gana", "gari", "gela", "gina", "gini", "goli", "grima", 

	"i", "ia", "ida", "ila", "ilda", "ili", "ika", "ina", "iri",

	"ka", "ki", "kia", "kona", "kuni",
	"la", "lani", "leda", "lena", "lia", "lina", "lona",
	"ma", "mani", "mela", "mina", "moda", "modi",
	"na", "neva", "ni", "nia", "nomi",
	"ola", "olga", "ona", "ondi", "oti", "ova",

	"ra", "raka", "rana", "ravi", "ri", "ria", "rimi", "rinda", "rundi", 
	"sha", "shi", "si", "ska",
	"ta", "tha", "ti", "tila", "tri", "tria", 
	"umma", "undi", "unga", "unni", 
	"vala", "vali", "vara", "vari", "vina",
	"ya",
	"zadi", "zani", "zara", "zi"
};
local male_secondpart = {
	"aim", "ain", "ak", "ald", "am", "ar", "ard", "ash", 
	"bain", "bald", "ban", "bar", "bash", "brun", "bur", 

	"dain", "dall", "dar", "din", "dok", "dol", "dor", "dum", "dun", "dur",
	"eff", "esh", "ev",
	"far", "fath", "feb",
	"gan", "gar", "gath", "gin", "gor", "grim", 

	"ik", "il", "im", "in", "ip", "isch", 

	"kad", "kar", "kash", "keld", "kesh", "kon", 
	"lek", "lem", "lesh", "let", "leth", "lin", "lor", 
	"mar", "mek", "min", "mok", "mon", "moth", "mund", 
	"nar", "nek", "nir", "nod", "nor", 
	"od", "oin", "ok", "old", "on", "ond", "or", 

	"rak", "ram", "rik", "rim", "rin", "ros", "roth", "rund", 
	"skal", "skar",
	"tar", "tek", "til", "tok", "tul",
	"um", "und", "unn", "ur", "urd",
	"val", "van", "var", "vath", "vek", "ven", "vesh", "veth", "vin", "von",

	"zad", "zar", "zek", "zim", "zin"
};

local dwarfRace = df.global.ui.race_id;

math.randomseed(tonumber(df.global.world.worldgen.worldgen_parms.name_seed,36));
for index,unit in pairs(df.global.world.history.figures) do --Renaming all the historical dwarves
	if ( unit.race == dwarfRace ) then
		if (unit.sex ~= 0) then
			unit.name.first_name = firstnamepart[math.random(#firstnamepart)]..male_secondpart[math.random(#male_secondpart)];
		else
			unit.name.first_name = firstnamepart[math.random(#firstnamepart)]..female_secondpart[math.random(#female_secondpart)];
		end
	end
end
for index,unit in pairs(df.global.world.units.all) do --Renaming pretty much everyone else
	if ( unit.race == dwarfRace ) then
		if unit.flags1.important_historical_figure or unit.flags2.important_historical_figure then
			--Use the same name as the histfig data
			unit.name.first_name = df.historical_figure.find(unit.hist_figure_id).name.first_name;
		else
			if (unit.sex ~= 0) then
				unit.name.first_name = firstnamepart[math.random(#firstnamepart)]..male_secondpart[math.random(#male_secondpart)];
			else
				unit.name.first_name = firstnamepart[math.random(#firstnamepart)]..female_secondpart[math.random(#female_secondpart)];
			end
		end
	end
end
print("firstnamepart: "..#firstnamepart.."\nfemale: "..#female_secondpart.."\nmale: "..#male_secondpart);