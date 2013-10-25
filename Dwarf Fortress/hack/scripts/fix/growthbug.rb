# growthbug  enable [#d|w|m] |disable|status|now|help
DAY = 1200
USAGE = <<ENDUSAGE
Usage:
   fix/growthbug  enable|start [#d|#w|#m]
                           start auto fix process with #d (number of days), 
                           or #w (number of weeks), or #m (number of months),
                           or the default of one month if omitted.
   fix/growthbug  disable|stop|end     stop auto fix process.
   fix/growthbug  now                  run fix process once.
   fix/growthbug  [help|?]             show this help.
   fix/growthbug  status
                           show auto fix status (running? not running?).
                           status is also shown with help, and when 
                           starting or stopping auto fix.
       (all options are case insensitive.)
ENDUSAGE

class FixGrowth
	def initialize()	@running = false end
	def status()		@running ? 'FixGrowth: Running' : 'FixGrowth: Stopped'	end
	def process
		count = 0
		df.world.units.active.each do |unit|
			r = unit.relations.birth_time % 10
			if unit.relations.birth_time > 0 and r > 0 then
				unit.relations.birth_time -= r
				count += 1
			end
		end
		puts "FixGrowth: Fixed #{count} units so that their size will grow/thicken." unless count == 0
	end
	
	def start( time=DAY*28 )
		self.stop if @running
		@onupdate = df.onupdate_register('fixgrowth', time) { process }
		@running = true
	end
	
	def stop
		df.onupdate_unregister(@onupdate)
		@running = false
	end
end	


$FixGrowth = FixGrowth.new unless $FixGrowth
run = :help
time = DAY * 28
$script_args.each do |arg|
	case arg.downcase
	when 'now'
		run = :now
	when 'enable', 'start'
		run = :enable
	when 'disable', 'end', 'stop'
		run = :disable
	when 'status'
		run = :status
	when 'help', '?'
		run = :help
	else
		if arg =~ /^([1-9]\d*)([dD|wW|mM])$/
			time =  $~[1].to_i
			case $~[2]
			when 'd', 'D'
				time *= DAY
			when 'w', 'W'
				time *= DAY * 7
			when 'm', 'M'
				time *= DAY * 28
			end
		end
	end
end
case run
when :now
	$FixGrowth.process
when :enable
	$FixGrowth.start(time)
	puts $FixGrowth.status
when :disable
	$FixGrowth.stop
	puts $FixGrowth.status
when :help
	puts $FixGrowth.status
	puts USAGE
end