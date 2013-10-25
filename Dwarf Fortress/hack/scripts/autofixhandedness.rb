class AutoFixHandedness

	def initialize
	end

	def process
		return false unless @running

		# the colection of GLOVES
		gloves=df.world.items.other[:GLOVES]
		hand=0 # which hand  0=right, 1=left
		count=0
		gloves.each do |glove|
			# if glove is unhanded (right and left = false, fix it
			unless glove.handedness[0] or glove.handedness[1]
				#puts glove.id 
				glove.handedness[hand] = true
				hand ^= 1 # switch hand for next glove
				count += 1
			end
		end

		puts "Found #{count} unhanded glove(s)." unless count == 0
		
	end
	
	def start
		@onupdate = df.onupdate_register('autofixhandedness', 2000) { process }
		@running = true
		@item_next_id = 0
	end
	
	def stop
		df.onupdate_unregister(@onupdate)
		@running = false
	end
	
	def status
		@running ? 'Running.' : 'Stopped.'
	end
		
end	

case $script_args[0]
when 'start'
    $AutoFixHandedness = AutoFixHandedness.new unless $AutoFixHandedness
    $AutoFixHandedness.start

when 'end', 'stop'
    $AutoFixHandedness.stop
	
else
    if $AutoFixHandedness
        puts $AutoFixHandedness.status
    else
        puts 'Not loaded.'
    end
end