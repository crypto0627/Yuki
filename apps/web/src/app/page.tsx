import Banner from '@/components/Banner';
import People from '@/components/People';
import Features from '@/components/Features';
import Business from '@/components/Business';
import Payment from '@/components/Payment';
import Pricing from '@/components/Pricing';


export default function Home() {
  return (
    <main>
      <Banner />
      <People />
      <Features />
      <Business />
      <Payment />
      <Pricing />
    </main>
  )
}
